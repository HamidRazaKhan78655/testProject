using RestSharp;
using System.Data.SqlClient;
//var client = new RestClient("http://localhost:51409/AllReports/AppraisalReportAnon.aspx?companyCode=tdp2329");

var client = new RestClient("https://tms.bepms.com/AllReports/AppraisalReportAnon.aspx?companyCode=tdp2329");


/**
 * Parameter List 
 * memberCode
 * memberName
 * period
 * reviewType
 * MemberId
 * requestId
 * reqReId
 * 
 */
using (SqlConnection conn = new SqlConnection())
{
    conn.ConnectionString = "uid=sa;pwd=GJ##nByr%n$U;Initial Catalog=BEPMS_TMS138;Data Source=172.18.9.107\\SQLBE";
    conn.Open();
    SqlCommand cmd = new SqlCommand(@"select * from AppraisalExport where IsProcessed = 0", conn);
    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
    {
        System.Data.DataTable dt = new System.Data.DataTable();
        adapter.Fill(dt);
        int i = 0;
        while (dt.Rows.Count > i)
        {
            var reader = dt.Rows[i];
            string memberCode, memberName, membersId, period, requestId, reqReId, reviewType, Id;
            memberCode = reader[0].ToString();
            memberName = reader[1].ToString();
            membersId = reader[2].ToString();
            period = reader[3].ToString();
            requestId = reader[4].ToString();

            reqReId = reader[5].ToString();
            reviewType = reader[6].ToString();
            string rowId = reader[8].ToString();

            var request = new RestRequest("http://tms.bepms.com/AllReports/AppraisalReportAnon.aspx?companyCode=tdp2238", Method.Post);
            request.AddQueryParameter("memberCode", memberCode);
            request.AddQueryParameter("memberName", memberName);
            request.AddQueryParameter("period", period);
            request.AddQueryParameter("MemberId", membersId);
            request.AddQueryParameter("requestId", requestId);
            request.AddQueryParameter("reqReId", reqReId);
            request.AddQueryParameter("reviewType", reviewType);
            request.AddQueryParameter("Id", reqReId);
            request.Timeout = Int32.MaxValue;
            var responseRest = (client.Execute(request));
            var response =  responseRest.ResponseStatus.ToString();
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            if (response != "Error") {
                var cmd2 = new SqlCommand("update AppraisaLExport set IsProcessed = 1 where Id = " + rowId, conn);
                cmd2.ExecuteNonQuery();
            }
            i++;
        }
    }
    // using the code here...  
}



