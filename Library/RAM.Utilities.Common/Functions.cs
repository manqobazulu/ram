using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;

using System.IO;
using System.Data;

namespace RAM.Utilities.Common
{
    public class Functions {
       

          #region Safe Methods

    public static bool IsNumeric(object s) {
        try { Int32.Parse(Convert.ToString(s)); }
        catch { return false; }
        return true;
    }

    public static bool IsDateTime(string s) {
        try { DateTime.Parse(s); }
        catch { return false; }
        return true;
    }

    public static string Truncate(string value, int MaxLength) {
        if (value.Length > MaxLength) value = value.Substring(0, MaxLength);
        return value;
    }

    public static int SafeInt(object value) {
        return SafeInt(value, 0);
    }

    public static int SafeInt(object value, int defaultValue) {
        try {
            if (SafeString(value).ToUpper() == "TRUE") return 1;
            if (SafeString(value).ToUpper() == "FALSE") return 0;
            return Convert.ToInt32(SafeDouble(value, defaultValue));
        }
        catch (Exception exc) {
            string s = exc.Message;
            return defaultValue;
        }
    }

    public static double SafeDouble(object value) {
        return SafeDouble(value, 0);
    }

    public static double SafeDouble(object value, double defaultValue) {
        try {
            return Convert.ToDouble(value);
        }
        catch (Exception exc) {
            string s = exc.Message;
            return defaultValue;
        }
    }

    public static Int64 SafeInt64(object value) {
        return SafeInt64(value, 0);
    }

    public static Int64 SafeInt64(object value, Int64 defaultValue) {
        try {
            return Convert.ToInt64(value);
        }
        catch {
            return defaultValue;
        }
    }

    public static string SafeString(object value) {
        return SafeString(value, string.Empty);
    }

    public static string SafeString(object value, string defaultValue) {
        try {
            return Convert.ToString(value);
        }
        catch {
            return defaultValue;
        }
    }

    public static bool SafeBool(object value) {
        return SafeBool(value, false);
    }

    public static bool SafeBool(object value, bool defaultValue) {
        try {
            if (SafeString(value).ToUpper().Trim() == "TRUE") return true;
            if (SafeString(value).ToUpper().Trim() == "FALSE") return false;
            if (SafeString(value).ToUpper().Trim() == "1") return true;
            if (SafeString(value).ToUpper().Trim() == "0") return false;
            if (SafeString(value).ToUpper().Trim() == "YES") return true;
            if (SafeString(value).ToUpper().Trim() == "NO") return false;
            if (SafeString(value).ToUpper().Trim() == "Y") return true;
            if (SafeString(value).ToUpper().Trim() == "N") return false;
            return defaultValue;
        }
        catch {
            return defaultValue;
        }
    }

    public static DateTime SafeDateTime(object value) {
        return SafeDateTime(value, DateTime.Now);
    }

    public static DateTime SafeDateTime(object value, DateTime defaultValue) {
        try {
            
            System.DateTime retVal = Convert.ToDateTime(value);
            
            if(retVal < UnixEpoch)
            {
                return UnixEpoch;
            }
            return retVal;
        }
        catch {
            return defaultValue;
        }
    }

    public static string SafeISODateTime(object value) {
        return SafeDateTime(value).ToString("yyyy-MM-dd HH:mm");
    }

    public static string SafeISOTime(object value) {
        return SafeDateTime(value).ToString("HH:mm:ss");
    }

    public static string SafeISODateTime(object value, DateTime defaultValue) {
        return SafeDateTime(value, defaultValue).ToString("yyyy-MM-dd HH:mm");
    }

    #endregion

    #region UnixTime

    public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);

    public static DateTime DateTimeFromUnix(object seconds) {
        return UnixEpoch + new TimeSpan(SafeInt64(seconds, 0) * TimeSpan.TicksPerSecond);
    }

    public static string DateTimeToUnix(DateTime value) {
        return ((Int64)((TimeSpan)(value - UnixEpoch)).TotalSeconds).ToString();
    }

    public static string DateTimeToString(DateTime value) {
        return value.ToString("yyyy-MM-dd HH:mm:ss");
    }

    #endregion

    public static byte[] ConvertNonSeekableStreamToByteArray(Stream NonSeekableStream) {
        MemoryStream ms = new MemoryStream();
        byte[] buffer = new byte[1024];
        int bytes;
        while ((bytes = NonSeekableStream.Read(buffer, 0, buffer.Length)) > 0) {
            ms.Write(buffer, 0, bytes);
        }
        byte[] output = ms.ToArray();
        return output;
    }

    public static byte[] StringToByteArray(string str) {
        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        return enc.GetBytes(str);
    }

    #region Select DISTINCT on DataTable

    public static DataTable SelectDistinct(DataTable SourceTable, params string[] FieldNames) {
        object[] lastValues;
        DataTable newTable;
        DataRow[] orderedRows;

        if (FieldNames == null || FieldNames.Length == 0)
            throw new ArgumentNullException("FieldNames");

        lastValues = new object[FieldNames.Length];
        newTable = new DataTable();

        foreach (string fieldName in FieldNames)
            newTable.Columns.Add(fieldName, SourceTable.Columns[fieldName].DataType);

        orderedRows = SourceTable.Select("", string.Join(", ", FieldNames));

        foreach (DataRow row in orderedRows) {
            if (!fieldValuesAreEqual(lastValues, row, FieldNames)) {
                newTable.Rows.Add(createRowClone(row, newTable.NewRow(), FieldNames));

                setLastValues(lastValues, row, FieldNames);
            }
        }

        return newTable;
    }

    private static bool fieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] fieldNames) {
        bool areEqual = true;
        for (int i = 0; i < fieldNames.Length; i++) {
            if (lastValues[i] == null || !lastValues[i].Equals(currentRow[fieldNames[i]])) {
                areEqual = false;
                break;
            }
        }
        return areEqual;
    }

    private static DataRow createRowClone(DataRow sourceRow, DataRow newRow, string[] fieldNames) {
        foreach (string field in fieldNames)
            newRow[field] = sourceRow[field];
        return newRow;
    }

    private static void setLastValues(object[] lastValues, DataRow sourceRow, string[] fieldNames) {
        for (int i = 0; i < fieldNames.Length; i++)
            lastValues[i] = sourceRow[fieldNames[i]];
    }

    #endregion
}

public class MultiStream : Stream {
    ArrayList streamList = new ArrayList();
    long position = 0;
    public override bool CanRead {
        get { return true; }
    }

    public override bool CanSeek {
        get { return true; }
    }

    public override bool CanWrite {
        get { return false; }
    }

    public override long Length {
        get {
            long result = 0;
            foreach (Stream stream in streamList) {
                result += stream.Length;
            }
            return result;
        }
    }

    public override long Position {
        get { return position; }
        set { Seek(value, SeekOrigin.Begin); }
    }

    public override void Flush() { }

    public override long Seek(long offset, SeekOrigin origin) {
        long len = Length;
        switch (origin) {
            case SeekOrigin.Begin:
                position = offset;
                break;
            case SeekOrigin.Current:
                position += offset;
                break;
            case SeekOrigin.End:
                position = len - offset;
                break;
        }
        if (position > len) {
            position = len;
        }
        else if (position < 0) {
            position = 0;
        }
        return position;
    }

    public override void SetLength(long value) { }

    public void AddStream(Stream stream) {
        streamList.Add(stream);
    }

    public override int Read(byte[] buffer, int offset, int count) {
        long len = 0;
        int result = 0;
        int buf_pos = offset;
        int bytesRead;
        foreach (Stream stream in streamList) {
            if (position < (len + stream.Length)) {
                stream.Position = position - len;
                bytesRead = stream.Read(buffer, buf_pos, count);
                result += bytesRead;
                buf_pos += bytesRead;
                position += bytesRead;
                if (bytesRead < count) {
                    count -= bytesRead;
                }
                else {
                    break;
                }
            }
            len += stream.Length;
        }
        return result;
    }

    public override void Write(byte[] buffer, int offset, int count) {

    }
}

public class ConcatMemoryStream {

    private MemoryStream outputStream = null;

    private Document newDocument = null;

    /// <summary>
    /// Combines the specified list of MemoryStream objects into one.
    /// </summary>
    /// <param name="sourceFiles">The source objects.</param>
    /// <returns>MemoryStream</returns>
    public MemoryStream Concat(List<MemoryStream> sourceFiles) {
        try {
            newDocument = new Document();
            outputStream = new MemoryStream();
            PdfSmartCopy pdf = new PdfSmartCopy(newDocument, outputStream);
            newDocument.Open();
            newDocument.AddTitle("RAM Online Waybill");

            foreach (MemoryStream ms in sourceFiles) {
                PdfReader pdfReader = new PdfReader(ms.ToArray());
                pdf.AddPage(pdf.GetImportedPage(pdfReader, 1));
            }
        }
        catch (Exception e) {
            throw new Exception(e.Message);
        }
        finally {            
            if (newDocument != null)
                newDocument.Close();
            //outputStream.Flush();
            //outputStream.Close();
        }
        return outputStream;
    }

    public void CloseObjects() {
        if (newDocument != null) {
            newDocument.Close();
        }
        outputStream.Flush();
        outputStream.Close();
        
        // ADDED: 30 April 2013
        outputStream.Dispose();
    }
}

public enum ConsignmentType {
    NORMAL = 0,
    RICA = 1,
    MULTIPAGE = 2
}
       
}

