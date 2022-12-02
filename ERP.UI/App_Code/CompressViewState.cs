using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.IO.Compression;

/// <summary>
/// Summary description for CompressViewState
/// </summary>
public class CompressViewState :System.Web.UI.Page
{
	public CompressViewState()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    protected override object LoadPageStateFromPersistenceMedium()
    {
        System.Web.UI.PageStatePersister pageStatePersister1 = this.PageStatePersister;
        pageStatePersister1.Load();
        String vState = pageStatePersister1.ViewState.ToString();
        byte[] pBytes = System.Convert.FromBase64String(vState);
        pBytes = Decompress(pBytes);
        LosFormatter mFormat = new LosFormatter();
        Object ViewState = mFormat.Deserialize(System.Convert.ToBase64String(pBytes));
        return new Pair(pageStatePersister1.ControlState, ViewState);
    }

    protected override void SavePageStateToPersistenceMedium(Object pViewState)
    {
          Pair pair1;
            System.Web.UI.PageStatePersister pageStatePersister1 = this.PageStatePersister;
            Object ViewState;
            if (pViewState is Pair)
            {
                pair1 = ((Pair)pViewState);
                pageStatePersister1.ControlState = pair1.First;
                ViewState = pair1.Second;
            }
            else
            {
                ViewState = pViewState;
            }
            LosFormatter mFormat = new LosFormatter();
            StringWriter mWriter = new StringWriter();
            mFormat.Serialize(mWriter, ViewState);
            String mViewStateStr = mWriter.ToString();
            byte[] pBytes = System.Convert.FromBase64String(mViewStateStr);
            pBytes = Compress(pBytes);
            String vStateStr = System.Convert.ToBase64String(pBytes);
            pageStatePersister1.ViewState = vStateStr;
            pageStatePersister1.Save();
    }
    /// <summary>
    /// This Methhod takes the byte stream as parameter 
    /// and return a compressed bytestream.
    /// For compression it uses GZipStream
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    private byte[] Compress(byte[] b)
    {
        MemoryStream ms = new MemoryStream();
        GZipStream zs = new GZipStream(ms, CompressionMode.Compress, true);
        zs.Write(b, 0, b.Length);
        zs.Close();
        return ms.ToArray();
    }

    /// <summary>
    /// This methhod takes the compressed byte stream as parameter
    /// and return a decompressed bytestream.
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    private byte[] Decompress(byte[] b)
    {
        MemoryStream ms = new MemoryStream();
        GZipStream zs = new GZipStream(new MemoryStream(b), CompressionMode.Decompress, true);
        byte[] buffer = new byte[4096];
        int size;
        while (true)
        {
            size = zs.Read(buffer, 0, buffer.Length);
            if (size > 0) 
                ms.Write(buffer, 0, size);
            else break;
        }
        zs.Close();
        return ms.ToArray();
    }

}
