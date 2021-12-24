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

        private static void SaveEntity<I>(ISqlTable<I> entity)
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

        private static bool TableExists(string name)
        {
            bool exists = false;    
            CreateQuery($"SELECT 1 FROM sqlite_master WHERE type='table' AND name='{name}'", cmd => 
            {
                cmd.ExecuteNonQuery();
                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    exists = reader.Read();
                }
            });
            return exists;
        }

        private static T LoadOne<T, K>(K id, string keyName, bool createIfnone) where T : class, ITable, new()
        {
            Type type = typeof(T);
            bool found = false;

            T entry = new T();

            TableInfo table = GetTableInfo(type);

            CreateQuery(string.Format(table.select, keyName), cmd =>
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
                    ReadEntry(entry, table, reader);
                }
            });

            if (!found && createIfnone)
            {
                if (entry is ISqlTable<K> ken) ken.Id = id;
                entry.Save();
                return entry;
            }

            return found ? entry : null;
        }

        private static T[] LoadAll<T, K>(K id, string keyName) where T : class, ITable, new()
        {
            Type type = typeof(T);
            TableInfo table = GetTableInfo(type);
            List<T> entries = new List<T>();
            CreateQuery(string.Format(table.select, keyName), cmd =>
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
                    while (reader.Read())
                    {
                        T entry = new T();
                        ReadEntry(entry, table, reader);
                        entries.Add(entry);
                    }
                }
            });

            return entries.ToArray();
        }

        private static void DeleteEntity<I>(ISqlTable<I> entry)
        {
            Type type = entry.GetType();
            if (!TableExists(type.Name)) return;
            CreateQuery($"DELETE FROM {type.Name} WHERE Id=@Id", cmd =>
            {
                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Id",
                    Value = entry.Id
                });

                cmd.ExecuteNonQuery();
            });
        }

        private static void DeleteAll<T, K>(K id, string keyName)
        {
            Type type = typeof(T);
            if (!TableExists(type.Name)) return;
            CreateQuery($"DELETE FROM {type.Name} WHERE {keyName}=@id", cmd =>
            {
                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Id",
                    Value = id
                });

                cmd.ExecuteNonQuery();
            });
        }

        private static void ReadEntry<T>(T entry, TableInfo table, SqliteDataReader reader)
        {
            Dictionary<string, object> values = GetColumnValues(reader);

            GetSetMember[] fields = table.fields;
            object v;
            for (int i = 0; i < fields.Length; i++)
            {
                GetSetMember field = fields[i];
                if (values.TryGetValue(field.Name, out v))
                    field.SetValue(entry, v);
            }

            if (values.TryGetValue(table.identifier.Name, out v))
            {
                table.identifier.SetValue(entry, v);
            }
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
            (bool create, bool rebuild) = VerifyTable(table);

            if (rebuild)
            {
                RebuildTable(table);
                return;
            }

            if (create) CreateQuery(table.create, cmd => cmd.ExecuteNonQuery());
        }

        private static void RebuildTable(TableInfo table)
        {
            List<Dictionary<string, object>> entries = new List<Dictionary<string, object>>();

            using (SqliteConnection connection = new SqliteConnection(dbPath))
            {
                connection.Open();
                using (SqliteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $"select * from {table.name}";
                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> entry = GetColumnValues(reader);
                            entries.Add(entry);
                        }
                    }

                    cmd.CommandText = $"drop table {table.name}";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = table.create;
                    cmd.ExecuteNonQuery();

                    if (entries.Count == 0) return;

                    cmd.CommandText = table.save;

                    for (int i = 0; i < entries.Count; i++)
                    {
                        Dictionary<string, object> entry = entries[i];

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "Id",
                            Value = entry[table.identifier.Name]
                        });

                        GetSetMember[] fields = table.fields;

                        for (int j = 0; j < fields.Length; j++)
                        {
                            GetSetMember field = fields[j];
                            Type t = field.ValueType;

                            if (!entry.TryGetValue(field.Name, out object v)) v = GetDefault(t);
                            else if (!v.GetType().Equals(t))
                            {
                                try { v = Convert.ChangeType(v, t); }
                                catch(Exception) { v = GetDefault(t); }
                            }

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = field.Name,
                                Value = v
                            });
                        }

                        var result = cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private static object GetDefault(Type t)
            => t.IsValueType ? Activator.CreateInstance(t) : null;

        private static (bool, bool) VerifyTable(TableInfo table)
        {
            bool create = false;
            bool rebuild = false;
            CreateQuery($"PRAGMA table_info({table.name})", cmd =>
            {
                cmd.ExecuteNonQuery();
                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        create = true;
                        return;
                    }

                    Dictionary<string, Dictionary<string, object>> data = new Dictionary<string, Dictionary<string, object>>();
                    do
                    {
                        Dictionary<string, object> values = GetColumnValues(reader);
                        data.Add(values["name"].ToString(), values);
                    }
                    while (reader.Read());

                    if (data.TryGetValue(table.identifier.Name, out Dictionary<string, object> idf))
                    {
                        string type = idf["type"].ToString();
                        if (type.Equals(SqlType(table.identifier.ValueType), StringComparison.OrdinalIgnoreCase))
                            data.Remove(table.identifier.Name);
                        else rebuild = true;
                    }
                    else rebuild = true;

                    for (int i = 0; !rebuild && i < table.fields.Length; i++)
                    {
                        GetSetMember mem = table.fields[i];

                        if (data.TryGetValue(mem.Name, out Dictionary<string, object> f))
                        {
                            string type = f["type"].ToString();
                            if (type.Equals(SqlType(mem.ValueType), StringComparison.OrdinalIgnoreCase))
                                data.Remove(mem.Name);
                            else rebuild = true;
                        }
                        else rebuild = true;
                    }

                    if (data.Count > 0) //There are extra columns
                        rebuild = true;
                }
            });

            return (create, rebuild);
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

        private static string SqlType(Type type)
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
            public MemberInfo Member => (isField ? field : (MemberInfo)prop);

            public bool IsNotSerializable => isField ? field.IsNotSerialized 
                : !(prop.SetMethod.IsPublic && prop.GetMethod.IsPublic);

            private readonly bool isField;
            private readonly FieldInfo field;
            private readonly PropertyInfo prop;

            private Type Parent => isField ? field.DeclaringType : prop.DeclaringType;

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
                if (value.GetType() == typeof(DBNull))
                    value = null;

                if (ValueType == typeof(int))
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
             => $"{Parent.Name}.{Name} ({ValueType.Name})";
        }

        private class TableInfo
        {
            public readonly string name;

            public readonly GetSetMember identifier;

            public readonly GetSetMember[] fields;

            public readonly string create;
            public readonly string select;
            public readonly string save;

            public TableInfo(Type type)
            {
                name = type.Name;
                identifier = type.GetProperty("Id");
                fields = LoadSerializableFields(type);

                select = $"SELECT * FROM {type.Name} WHERE {{0}} = @Id";

                create = $"CREATE TABLE '{type.Name}' ("
                + $"'Id' {SqlType(identifier.ValueType)} PRIMARY KEY {HasIncrement(type)},"
                + JoinFields(",", fields, field => $" '{field.Name}' {SqlType(field.ValueType)}{HasIncrement(field.Member)}")
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
                    GetSetMember member = members[i];
                    if (member.Name == "Id" || member.IsNotSerializable) continue;
                    Type type = member.ValueType;
                    if (type.IsPrimitive || type == typeof(string) || type == typeof(DateTime)) serialized.Add(member);
                }

                return serialized.ToArray();
            }

            private string HasIncrement(MemberInfo member)
            {
                if (member.IsDefined(typeof(AutoIncrement)))
                    return "AUTOINCREMENT";
                return null;
            }
        }

        public interface ITable
        {
            void Save();
        }

        public interface ISqlTable<K>
        {
            K Id { get; set; }
        }

        public abstract class SqlTable<TKey> : ISqlTable<TKey>, ITable
        {
            public static TEntry LoadOne<TEntry>(TKey id, bool createIfNone = false) 
                where TEntry : class, ITable, new() => LoadOne<TEntry, TKey>(id, "Id", createIfNone);

            public static TEntry[] LoadAll<TEntry>(TKey id, string keyName = "Id")
                where TEntry : class, ITable, new() => SqliteHandler.LoadAll<TEntry, TKey>(id, keyName);

            public static TEntry[] LoadAll<TEntry, TForeign>(TForeign id, string keyName = "ForeignKey")
                where TEntry : class, ITable, new() => SqliteHandler.LoadAll<TEntry, TForeign>(id, keyName);

            public static void DeleteAll<TEntry, TForeign>(TForeign id, string keyName = "Id")
                where TEntry : class, ITable, new() => SqliteHandler.DeleteAll<TEntry, TForeign>(id, keyName);

            public TKey Id { get; set; }

            public virtual void Save() => SaveEntity(this);

            public virtual void Delete() => DeleteEntity(this);
        } 

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class)]
        public class AutoIncrement : Attribute { }
    }
}
