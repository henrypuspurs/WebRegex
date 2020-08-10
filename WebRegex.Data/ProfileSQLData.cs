using System;
using System.Collections.Generic;
using System.Text;
using WebRegex.Core.Models;
using Dapper;
using System.Data;
using System.Runtime.CompilerServices;
using System.Linq;

namespace WebRegex.Data
{
    public class ProfileSQLData
    {
        public List<Profile> GetProfiles(string connectionString)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                return connection.Query<Profile>("select * from Profiles").ToList();
            }
        }

        public List<Expression> GetExpressions(int profileId, string connectionString)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                return connection.Query<Expression>($"select * from Expressions where ProfileId = {profileId}").ToList();
            }
        }
    }
}
