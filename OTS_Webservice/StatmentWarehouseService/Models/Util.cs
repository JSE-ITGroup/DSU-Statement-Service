using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Reflection;
using System.Data;

namespace StatmentWarehouseService.Models
{
   
        static class Utils
        {
            internal static string GenerateUniqueReference()
            {
                string str = Guid.NewGuid().ToString();
                str = str.Substring(str.Length - 20, 20);
                return str;
            }
            internal static DataSet unZipDS(byte[] bdata, string schema)
            {
                DataSet _data = new DataSet();
                _data.ReadXmlSchema(new StringReader(schema));
                if (bdata != null)
                {
                    MemoryStream ms2 = new MemoryStream(bdata);
                    GZipStream zipStream = new GZipStream(ms2, CompressionMode.Decompress);
                    _data.ReadXml(zipStream, XmlReadMode.Auto);
                    _data.AcceptChanges();
                    zipStream.Close();
                    zipStream.Dispose();
                    ms2.Close();
                    ms2.Dispose();
                }
                return _data;
            }
            internal static string MD5(string input)
            {
                input = input ?? String.Empty;
                byte[] data = new byte[input.Length];
                Encoding.Default.GetBytes(input, 0, input.Length, data, 0); // set byte form of input
                MD5CryptoServiceProvider crypto = new MD5CryptoServiceProvider();
                byte[] result = crypto.ComputeHash(data); // compute MD5 value
                return byteArrayToHexString(result);
            }
            private static string byteArrayToHexString(byte[] byteArray)
            {
                StringBuilder output = new StringBuilder(byteArray.Length);
                foreach (byte b in byteArray)
                    output.AppendFormat("{0:x2}", b);
                return output.ToString();
            }
            public static string DataSetToXMLStr(DataSet dsSrc)
            {
                DataSet ds = dsSrc.Copy();
                foreach (DataTable t in ds.Tables)
                {
                    //add IUD
                    if (!t.Columns.Contains("IUD"))
                        t.Columns.Add("IUD", System.Type.GetType("System.String"));

                    //remove not null
                    foreach (DataColumn c in t.Columns)
                        if (c.AllowDBNull == false)
                            c.AllowDBNull = true;
                }
                foreach (DataTable t in ds.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        if (r.RowState == DataRowState.Unchanged)
                            continue;
                        switch (r.RowState)
                        {
                            case DataRowState.Added:
                                r["IUD"] = "I";
                                break;
                            case DataRowState.Modified:
                                r["IUD"] = "U";
                                break;
                            case DataRowState.Deleted:
                                r.RejectChanges();
                                r["IUD"] = "D";
                                break;
                            default:
                                break;
                        }
                    }
                }
                //update parent records
                foreach (DataTable t in ds.Tables)
                {
                    if (t.ParentRelations.Count == 0)
                        continue;
                    foreach (DataRelation rel in t.ParentRelations)
                    {
                        foreach (DataColumn c in rel.ChildColumns)
                            if (!c.ExtendedProperties.ContainsKey("KEY"))
                                c.ExtendedProperties.Add("KEY", "Y");
                        foreach (DataColumn c in rel.ParentColumns)
                            if (!c.ExtendedProperties.ContainsKey("KEY"))
                                c.ExtendedProperties.Add("KEY", "Y");
                    }
                    foreach (DataRow r in t.Rows)
                    {
                        if (r.RowState == DataRowState.Unchanged)
                            continue;
                        foreach (DataRelation rel in t.ParentRelations)
                        {
                            DataRow pr = r.GetParentRow(rel);
                            if (pr.RowState == DataRowState.Unchanged && pr["IUD"] == DBNull.Value)
                                pr["IUD"] = "N";
                        }
                    }
                }
                foreach (DataTable t in ds.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        if (r.RowState == DataRowState.Deleted)
                            continue;
                        if (r["IUD"] == DBNull.Value)
                        {
                            r.Delete();
                            continue;
                        }
                        if ((String)r["IUD"] == "D" || (String)r["IUD"] == "N")
                        {
                            foreach (DataColumn c in t.Columns)
                            {
                                if (c.ColumnName == "IUD")
                                    continue;
                                if (c.ReadOnly)
                                    c.ReadOnly = false;
                                if (!c.ExtendedProperties.ContainsKey("KEY"))
                                    r[c] = DBNull.Value;
                            }
                            continue;
                        }
                    }
                }
                //clear all readonly
                foreach (DataTable t in ds.Tables)
                {
                    foreach (DataColumn c in t.Columns)
                    {
                        if (c.ReadOnly == false)
                            continue;
                        if (c.ExtendedProperties.ContainsKey("KEY"))
                            continue;
                        c.ReadOnly = false;
                        foreach (DataRow r in t.Rows)
                            if (r.RowState != DataRowState.Deleted)
                                r[c] = DBNull.Value;
                    }
                }
                ds.AcceptChanges();
                return ds.GetXml();
            }
            public static System.DateTime AddBusinessDays(this System.DateTime source, int businessDays)
            {
                var dayOfWeek = businessDays < 0
                                    ? ((int)source.DayOfWeek - 12) % 7
                                    : ((int)source.DayOfWeek + 6) % 7;

                switch (dayOfWeek)
                {
                    case 6:
                        businessDays--;
                        break;
                    case -6:
                        businessDays++;
                        break;
                }

                return source.AddDays(businessDays + ((businessDays + dayOfWeek) / 5) * 2);
            }
        }
    
}
