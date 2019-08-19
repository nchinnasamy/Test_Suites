using System;
using System.Data;
using System.Data.SqlClient;

namespace PayQuest_dataModels
{
    public class MartialStatus:MartialStatusModal
    {

        #region Queries -------------------------------------------------------

        private const string SELECT_ALL_QUERY = @"SELECT [MaritalStatusID], [Name], [Description], [Active], [UpdatedDate], [UpdatedByUserID] FROM [Config].[MaritalStatus]";

        private const string SELECT_SIMPLE_ALL_QUERY = @"SELECT [MaritalStatusID], [Name], [Description], [Active] FROM [Config].[MaritalStatus]";

        private const string SELECT_QUERY = SELECT_ALL_QUERY + @" WHERE [MaritalStatusID] = {0}";

        private const string LAST_IDENTIFIER_QUERY = @"SELECT TOP(1) [MaritalStatusID] FROM [Config].[MaritalStatus] ORDER BY [MaritalStatusID] DESC";

        #endregion Queries ----------------------------------------------------


        public enum MaritalStatuses : byte
        {
            /// <summary>
            ///		Unknown/NULL
            /// </summary>
            Unknown = 0,

            /// <summary>
            ///		Single
            /// </summary>
            Single = 1,

            /// <summary>
            ///		Married
            /// </summary>
            Married = 2,

            /// <summary>
            ///		Defacto
            /// </summary>
            Defacto = 3,

            /// <summary>
            ///		Divorced
            /// </summary>
            Divorced = 4
        }
        public const string ALIAS = "cmas";

        /// <summary>
        ///		The tablename
        /// </summary>
        public const string TABLENAME = "MaritalStatus";

        /// <summary>
        ///		The identifier
        /// </summary>
        public const string IDENTIFIER = "MaritalStatusID";

        /// <summary>
        ///		The name column
        /// </summary>
        public const string COL_NAME = "Name";

        /// <summary>
        ///		Gets the name of the key field.
        ///		<para>This will be the single field which uniquely identifies a
        ///		record. It may be the primary key or a unique index</para>
        /// </summary>
        /// <value>
        ///		The name of the key field.
        /// </value>
        public string KeyFieldName
        {
            get { return IDENTIFIER; }
        }

        public string TableName
        {
            get { return TABLENAME; }
        }

        /// <summary>
        ///		Gets the alias for a database object.
        ///		<note type="note">Alias must be unique among IFilterable objects</note>
        /// </summary>
        /// <value>
        ///		The alias.
        /// </value>
        public string Alias
        {
            get { return ALIAS; }
        }

        public void Insert(SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand("[Config].[usp_InsertMaritalStatus]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaritalStatusID", MaritalStatusID);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Description", Description);
                cmd.Parameters.AddWithValue("@Active", Active);
                cmd.Parameters.AddWithValue("@UpdatedByUserID", UpdatedByUserID);
                cmd.Parameters.AddWithValue("@UpdatedDate", UpdatedDate);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
