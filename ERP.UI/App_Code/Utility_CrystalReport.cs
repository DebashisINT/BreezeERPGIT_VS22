using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CrystalDecisions;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;

/// <summary>
/// Summary description for Utility_CrystalReport
/// </summary>
public class Utility_CrystalReport
{
	public Utility_CrystalReport()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    protected static Queue reportQueue = new Queue();
    protected static int iMaxCount = 75;

    protected static ReportDocument CreateReport(Type reportClass)
    {
        object report = Activator.CreateInstance(reportClass);
        reportQueue.Enqueue(report);
        return (ReportDocument)report;
    }

    public static ReportDocument GetReport(Type reportClass)
    {
        if (reportQueue.Count > iMaxCount)
        {
            ((ReportDocument)reportQueue.Dequeue()).Close();
            ((ReportDocument)reportQueue.Dequeue()).Dispose();
            GC.Collect();
        }
        return CreateReport(reportClass);
    }   

}





