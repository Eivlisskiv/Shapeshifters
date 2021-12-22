using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace IgnitedBox.SaveData.Databases.Sqlite
{
    public static class SqliteHandler
    {
        private static readonly string dbPath = "URI=file:" + Application.persistentDataPath + "/ql_dtbs.db";

        private static readonly Dictionary<Type, TableInfo> TableCache = new Dictionary<Type, TableInfo>();

        private static TableInfo GetTableInfo(Type type)
        {
            if (TableCache.TryGetValue(type, out TableInfo table))
                return table;

            table = new TableInfo(type);

            TableCache.Add(type, table);
            return table;
        }

        private static void SaveEntity<I>(SqlTable<I> entity)
        {
            Type type = entity.GetType();

            TableInfo table = GetTableInfo(type);

            CreateQuery(table.save, cmd =>
            {
                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Id",
                    Value = entity.Id
                });

                GetSetMember[] fields = table.fields;

                for (int i = 0; i < fields.Length; i++)
                {
                    GetSetMember field = fields[i];

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = field.Name,
                        Value = field.GetValue(entity)
                    });
                }

                var result = cmd.ExecuteNonQuery();
            });
        }

        private static T Load<T, I>(I id, bool createIfnone) where T : SqlTable<I>, new()
        {
            Type type = typeof(T);
            bool found = false;
            T entry = new T() { Id = id };

            TableInfo table = GetTableInfo(type);

            CreateQuery(table.select, cmd =>
            {
                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Id",
                    Value = id
                });

#pragma warning disable IDE0063 // Use simple 'using' statement
                using (SqliteDataReader reader = cmd.ExecuteReader())
#pragma warning restore IDE0063 // Use simple 'using' statement
                {
                    if (!reader.Read()) return;

                    found = true;
                    Dictionary<string, object> values = GetColumnValues(reader);

                    GetSetMember[] fields = table.fields;

                    for (int i = 0; i < fields.Length; i++)
                    {
                        GetSetMember field = fields[i];
                        if (values.TryGetValue(field.Name, out object v))
                            field.SetValue(entry, v);
                    }
                }
            });

            if (!found && createIfnone)
            {
                SaveEntity(entry);
                return entry;
            }

            return found ? entry : null;
        }

        private static Dictionary<string, object> GetColumnValues(SqliteDataReader reader)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                values.Add(reader.GetName(i), reader.GetValue(i));
            }
            return values;
        }

        private static void CreateTable(TableInfo table)
        {
            //Check if table needs updating

            CreateQuery(table.create, cmd =>
            {
                var result = cmd.ExecuteNonQuery();
            });
        }

        private static string JoinFields(string seperator, GetSetMember[] fields, Func<GetSetMember, string> parse)
        {
            string str = null;
            for (int i = 0; i < fields.Length; i++)
            {
                GetSetMember field = fields[i];
                str += parse(field);
                if (i + 1 < fields.Length) str += seperator;
            }
            return str;
        }

        private static object SqlType(Type type)
        {
            if (type == typeof(string) || type == typeof(char))
                return "TEXT";

            if (type == typeof(short) || type == typeof(int) || type == typeof(long))
                return "INTEGER";
                
            if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
                return "REAL";

            if (type == typeof(decimal))
                return "NUMERIC";

            return "TEXT";
        }

        private static void CreateQuery(string query, Action<SqliteCommand> action)
        {
            UseConnection(connection =>
            {
#pragma warning disable IDE0063 // Use simple 'using' statement
                using (SqliteCommand cmd = connection.CreateCommand())
#pragma warning restore IDE0063 // Use simple 'using' statement
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = query;
                    action(cmd);
                }
            });
        }

        private static void UseConnection(Action<SqliteConnection> action)
        {
#pragma warning disable IDE0063 // Use simple 'using' statement
            using (SqliteConnection connection = new SqliteConnection(dbPath))
#pragma warning restore IDE0063 // Use simple 'using' statement
            {
                connection.Open();
                action(connection);
            }
        }

        private class GetSetMember
        {
            public static implicit operator GetSetMember(MemberInfo member)
                => new GetSetMember(member);

            public string Name => isField ? field.Name : prop.Name;
            public Type ValueType => isField ? field.FieldType : prop.PropertyType;

            private readonly bool isField;
            private readonly FieldInfo field;
            private readonly PropertyInfo prop;

            private Type parent => isField ? field.DeclaringType : prop.DeclaringType;

            public GetSetMember(MemberInfo member)
            {
                if (member is FieldInfo field)
                {
                    isField = true;
                    this.field = field;
                }
                else
                {
                    prop = (PropertyInfo)member;
                    isField = false;
                }
            }

            public object GetValue(object instance)
                => isField ? field.GetValue(instance) : prop.GetValue(instance);

            public void SetValue(object instance, object value)
            {
                if(ValueType == typeof(int))
                {
                    value = Convert.ToInt32((long)value);
                }

                else if (ValueType == typeof(short))
                {
                    value = Convert.ToInt16((long)value);
                }

                if (isField) field.SetValue(instance, value);
                else prop.SetValue(instance, value);
            }

            public override string ToString()
             => $"{parent.Name}.{Name} ({ValueType.Name})";
        }

        private class TableInfo
        {
            public readonly FieldInfo identifier;

            public readonly GetSetMember[] fields;

            public readonly string create;
            public readonly string select;
            public readonly string save;

            public TableInfo(Type type)
            {
                identifier = type.GetField("Id");
                fields = LoadSerializableFields(type);

                select = $"SELECT * FROM {type.Name} WHERE Id = @Id";

                create = $"CREATE TABLE IF NOT EXISTS '{type.Name}' ("
                + $"'Id' {SqlType(identifier.FieldType)} PRIMARY KEY,"
                + JoinFields(",", fields, field => $" '{field.Name}' {SqlType(field.ValueType)}")
                + ");";

                save = "INSERT or REPLACE INTO " + $"{type.Name} " +
                    $"(Id{JoinFields("", fields, f => ", " + f.Name)}) " +
                    $"VALUES (@Id{JoinFields("", fields, f => $", @{f.Name}")})";

                CreateTable(this);
            }

            private GetSetMember[] LoadSerializableFields(Type self)
            {
                const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

                List<MemberInfo> members = new List<MemberInfo>(self.GetFields(bindingFlags));
                members.AddRange(self.GetProperties(bindingFlags));
                List<GetSetMember> serialized = new List<GetSetMember>();
                for (int i = 0; i < members.Count; i++)
                {
                    MemberInfo member = members[i];
                    if (member.Name == "Id" || (member is FieldInfo field && field.IsNotSerialized))
                        continue;

                    serialized.Add(member);
                }

                return serialized.ToArray();
            }
        }

        public abstract class SqlTable<I>
        {
            public static T Load<T>(I id, bool createIfNone = false) 
                where T : SqlTable<I>, new()
                => Load<T, I>(id, createIfNone);

            public I Id;

            public void Save() => SaveEntity(this);
        } 
    }
}
