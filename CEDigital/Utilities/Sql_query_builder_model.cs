namespace CEDigital.Utilities
{
    // Clase para construir consultas SQL completas
    public class Sql_query_builder_model
    {
        private string table_name;
        private string[] column_names;
        private List<string> where_clauses;
        private List<string> join_clauses;
        private string order_by_clause;

        // Diccionarios para valores en INSERT y UPDATE
        private Dictionary<string, string> insert_values;
        private Dictionary<string, string> update_values;

        // Constructor
        public Sql_query_builder_model(string table_name, params string[] column_names)
        {
            this.table_name = table_name;
            this.column_names = column_names;
            this.where_clauses = new List<string>();
            this.join_clauses = new List<string>();
            this.order_by_clause = "";
            this.insert_values = new Dictionary<string, string>();
            this.update_values = new Dictionary<string, string>();
        }

        // Agrega condición WHERE
        public void add_where(string condition)
        {
            where_clauses.Add(condition);
        }

        // Agrega JOIN
        public void add_join(string join_type, string join_table, string on_condition)
        {
            join_clauses.Add($"{join_type} JOIN {join_table} ON {on_condition}");
        }

        // Define el ordenamiento
        public void set_order_by(string column_name, string order_direction = "ASC")
        {
            order_by_clause = $"ORDER BY {column_name} {order_direction}";
        }

        // Define valores para INSERT
        public void set_insert_value(string column, string value)
        {
            insert_values[column] = value;
        }

        // Define valores para UPDATE
        public void set_update_value(string column, string value)
        {
            update_values[column] = value;
        }

        // Genera SELECT
        public string build_select()
        {
            string column_part = (column_names.Length == 0) ? "*" : string.Join(", ", column_names);
            string query = $"SELECT {column_part} FROM {table_name}";

            if (join_clauses.Count > 0)
            {
                query += " " + string.Join(" ", join_clauses);
            }

            if (where_clauses.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", where_clauses);
            }

            if (!string.IsNullOrEmpty(order_by_clause))
            {
                query += " " + order_by_clause;
            }

            return query;
        }

        // Genera INSERT
        public string build_insert()
        {
            if (insert_values.Count == 0) return "// No se definieron valores para INSERT";

            string columns = string.Join(", ", insert_values.Keys);
            string values = string.Join(", ", insert_values.Values.Select(v => $"'{v}'"));
            return $"INSERT INTO {table_name} ({columns}) VALUES ({values})";
        }

        // Genera UPDATE
        public string build_update()
        {
            if (update_values.Count == 0) return "// No se definieron valores para UPDATE";

            string set_clause = string.Join(", ", update_values.Select(kv => $"{kv.Key} = '{kv.Value}'"));
            string query = $"UPDATE {table_name} SET {set_clause}";

            if (where_clauses.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", where_clauses);
            }

            return query;
        }

        // Genera DELETE
        public string build_delete()
        {
            string query = $"DELETE FROM {table_name}";

            if (where_clauses.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", where_clauses);
            }

            return query;
        }
    }
}
