using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
// ***********************************************************************
// This code was generated by DatAdmin DB MODEL Plugin, please do not edit
// ***********************************************************************
using System.Data.Common;
namespace Generated
{
    public static class DbCreator
    {
        public static string DBVERSION = "1";
        private static void ExecuteNonQuery(DbConnection conn, DbTransaction tran, string sql)
        {
            using (DbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
        }
        public static string GetVersion(DbConnection conn, DbTransaction tran)
        {
            try
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    cmd.CommandText = "SELECT Value FROM Variables WHERE Name=\'version\'";
                    return cmd.ExecuteScalar().ToString();
                }
            }
            catch
            {
                return null;
            }
        }
        // update to version 1
        private static void UpdateToVersion_0(DbConnection conn, DbTransaction tran)
        {
            ExecuteNonQuery(conn, tran, "CREATE TABLE \"Variables\" ( \n    \"Name\" text NOT NULL, \n    \"Value\" text NOT NULL,  \n    PRIMARY KEY (\"Name\")\n)");
            ExecuteNonQuery(conn, tran, "CREATE TABLE \"QueryExecute\" ( \n    \"ID\" integer primary key NOT NULL, \n    \"ExecutedAt\" datetime NOT NULL, \n    \"ExecutedAtTImestamp\" integer NOT NULL, \n    \"Dialect\" text NOT NULL, \n    \"QueryText_ID\" integer NOT NULL, \n    \"DbServer\" text NULL, \n    \"DbName\" text NULL, \n    \"QueryContext\" text NOT NULL, \n    \"FileName\" text NULL, \n    \"DurationInMilisecs\" integer NULL, \n    CONSTRAINT \"FK_QueryExecute_QueryText_ID\" FOREIGN KEY (\"QueryText_ID\") REFERENCES \"QueryText\"(\"ID\")\n)");
            ExecuteNonQuery(conn, tran, "CREATE TABLE \"QueryText\" ( \n    \"ID\" integer primary key NOT NULL, \n    \"QueryText\" text NOT NULL, \n    \"HashCode\" integer NOT NULL\n)");
            ExecuteNonQuery(conn, tran, "CREATE INDEX \"IX_QueryText_HashCode\" ON \"QueryText\" (\"HashCode\")");
            ExecuteNonQuery(conn, tran, "INSERT INTO \"Variables\" (\"Name\", \"Value\") VALUES (\'version\', \'0\')");
            ExecuteNonQuery(conn, tran, "UPDATE Variables SET Value=\'1\' WHERE Name=\'version\'");
        }
        public static void UpdateDb(DbConnection conn)
        {
            using (DbTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    if (GetVersion(conn, tran) == null) UpdateToVersion_0(conn, tran);
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                tran.Commit();
            }
        }
        public static void CheckVersion(DbConnection conn)
        {
            string realversion = GetVersion(conn, null);
            if (realversion != DBVERSION)
            {
                throw new Exception(String.Format("Bad version of database, code_version={0}, db_version={1}", DBVERSION, realversion));
            }
        }
    }
}
