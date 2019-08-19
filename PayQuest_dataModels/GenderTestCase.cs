using System;
using System.Data;
using System.Data.SqlClient;

namespace PayQuest_dataModels
{
   public  class GenderTestCase:Gender
    {
        #region Queries -------------------------------------------------------

        private const string SELECT_ALL_QUERY = @"SELECT [GenderID], [Name], [Description], [Active], [DefaultImage], [UpdatedDate], [UpdatedByUserID] FROM [Config].[Gender]";

        private const string SELECT_SIMPLE_ALL_QUERY = @"SELECT [GenderID], [Name], [Description], [Active], [DefaultImage] FROM [Config].[Gender]";

        private const string SELECT_QUERY = SELECT_ALL_QUERY + @" WHERE [GenderID] = {0}";

        private const string LAST_IDENTIFIER_QUERY = @"SELECT TOP(1) [GenderID] FROM [Config].[Gender] ORDER BY [GenderID] DESC";

        #endregion Queries ----------------------------------------------------

        /// <summary>
        ///		Config.Gender constants
        /// </summary>
        /// <remarks>
        /// 	THIS LIST DOSEN'T NECESSARILY HAVE TO MATCH THE FULL LIST; HOWEVER
        ///		ANY VALUES MUST MATCH THE IDENTIFIER IN THE DATABASE
        /// </remarks>
        public enum Genders : byte
        {
            /// <summary>
            ///		Unknown
            /// </summary>
            Unknown = 0,

            /// <summary>
            ///		Male
            /// </summary>
            Male = 1,

            /// <summary>
            ///		Female
            /// </summary>
            Female = 2
        }

        public const string ALIAS = "cg";

        /// <summary>
        ///		The tablename
        /// </summary>
        public const string TABLENAME = "Gender";

        /// <summary>
        ///		The identifier
        /// </summary>
        public const string IDENTIFIER = "GenderID";

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
            using (SqlCommand cmd = new SqlCommand("[Config].[usp_InsertGender]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GenderID", GenderID);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Description", Description);
                cmd.Parameters.AddWithValue("@DefaultImage", DefaultImage);
                cmd.Parameters.AddWithValue("@Active", Active);
                cmd.Parameters.AddWithValue("@UpdatedByUserID", UpdatedByUserID);
                cmd.Parameters.AddWithValue("@UpdatedDate", UpdatedDate);

                cmd.ExecuteNonQuery();
            }
        }

    }
}
