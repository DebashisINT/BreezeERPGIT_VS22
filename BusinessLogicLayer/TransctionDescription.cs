using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicLayer
{
   public class TransctionDescription
    {

       public string Description(string des)
       {
           /*
               nonMatchDescription,
               nonParseTransasction,
               CdslTransaction_InternalReference,
               CdslTransaction_SequenceNumber,
               CdslTransaction_DRNStatus,
               CdslTransaction_CCID,
               CdslTransaction_counterCMBPID,
               CdslTransaction_CounterBenID,
               CdslTransaction_CounterDPID,
               CdslTransaction_ExchangeID, 
               CdslTransaction_CMID,
               CdslTransaction_FreezeExpdt,
               CdslTransaction_counterSettlementId,
              */

           String retValue, initial,
                   nonMatchDescription,
                   nonParseTransasction,
                   CdslTransaction_InternalReference,
                   CdslTransaction_SequenceNumber,
                   CdslTransaction_DRNStatus,
                   CdslTransaction_CCID,
                   CdslTransaction_CounterCMBPID,
                   CdslTransaction_CounterBenID,
                   CdslTransaction_CounterDPID,
                   CdslTransaction_ExchangeID,
                   CdslTransaction_CMID,
                   CdslTransaction_FreezeExpdt,
                   CdslTransaction_CounterSettlementId;

           retValue = String.Empty;
           initial = String.Empty;

           nonMatchDescription = String.Empty;
           nonParseTransasction = String.Empty;

           CdslTransaction_InternalReference = String.Empty;
           CdslTransaction_SequenceNumber = String.Empty;
           CdslTransaction_DRNStatus = String.Empty;
           CdslTransaction_CCID = String.Empty;
           CdslTransaction_CounterCMBPID = String.Empty;
           CdslTransaction_CounterBenID = String.Empty;
           CdslTransaction_CounterDPID = String.Empty;
           CdslTransaction_ExchangeID = String.Empty;
           CdslTransaction_CMID = String.Empty;
           CdslTransaction_FreezeExpdt = String.Empty;
           CdslTransaction_CounterSettlementId = String.Empty;



           try
           {


               string flag = "0";

               initial = des.ToUpper();


               if ((initial.Substring(0, 3).Trim().Equals("BSE") ||
                          initial.Substring(0, 3).Trim().Equals("NSC") ||
                             initial.Substring(0, 3).Trim().Equals("CSE") ||
                               initial.Substring(0, 3).Trim().Equals("DSE") ||
                                   initial.Substring(0, 3).Trim().Equals("ASE") ||
                                       initial.Substring(0, 3).Trim().Equals("OTC") ||
                                           initial.Substring(0, 3).Trim().Equals("NCD") ||
                                               initial.Substring(0, 3).Trim().Equals("MCX") ||
                                                   initial.Substring(0, 3).Trim().Equals("BSL") ||
                                                  initial.Substring(0, 3).Trim().Equals("NSL")) && initial.Substring(3, 7).Equals("-PAYOUT"))
               {
                   string ccid = "", exid = "", name;
                   name = initial.Substring(0, 3);

                   if (name == "BSE")
                   {
                       ccid = "IN001019";
                       exid = "11";
                   }
                   else if (name == "NSC")
                   {
                       ccid = "IN001002";
                       exid = "12";
                   }
                   else if (name == "CSE")
                   {
                       ccid = "IN001027";
                       exid = "13";
                   }
                   else if (name == "DSE")
                   {
                       ccid = "IN001035";
                       exid = "14";
                   }
                   else if (name == "ASE")
                   {
                       ccid = "IN001094";
                       exid = "15";
                   }
                   else if (name == "OTC")
                   {
                       ccid = "IN001086";
                       exid = "19";
                   }
                   else if (name == "NCD")
                   {
                       ccid = "IN001109";
                       exid = "22";
                   }
                   else if (name == "MCX")
                   {
                       ccid = "IN002322";
                       exid = "23";
                   }
                   else if (name == "BSL")
                   {
                       ccid = "IN001021";
                       exid = "25";
                   }
                   else if (name == "NSL")
                   {
                       ccid = "IN001020";
                       exid = "24";
                   }



                   CdslTransaction_CCID = ccid;


                   CdslTransaction_CounterBenID = des.Substring(32, 16);
                   CdslTransaction_ExchangeID = exid;
                   CdslTransaction_CounterSettlementId = des.Substring(19, des.Substring(19).IndexOf(" "));

                   // retValue = ",,," + ccid + ",," + des.Substring(32, 16) + ",," + exid + ",,,"+ des.Substring(19, des.Substring(19).IndexOf(" "));

               }
               else if ((initial.Substring(0, 3).Trim().Equals("BSE") ||
                     initial.Substring(0, 3).Trim().Equals("NSC") ||
                        initial.Substring(0, 3).Trim().Equals("CSE") ||
                          initial.Substring(0, 3).Trim().Equals("DSE") ||
                              initial.Substring(0, 3).Trim().Equals("ASE") ||
                                  initial.Substring(0, 3).Trim().Equals("OTC") ||
                                      initial.Substring(0, 3).Trim().Equals("NCD") ||
                                          initial.Substring(0, 3).Trim().Equals("MCX") ||
                                              initial.Substring(0, 3).Trim().Equals("BSL") ||
                                                  initial.Substring(0, 3).Trim().Equals("NSL")) && initial.Substring(3, 8).Equals("EPPAYIN-"))
               {
                   string ccid = "", exid = "", name;
                   name = initial.Substring(0, 3);

                   if (name == "BSE")
                   {
                       ccid = "IN001019";
                       exid = "11";
                   }
                   else if (name == "NSC")
                   {
                       ccid = "IN001002";
                       exid = "12";
                   }
                   else if (name == "CSE")
                   {
                       ccid = "IN001027";
                       exid = "13";
                   }
                   else if (name == "DSE")
                   {
                       ccid = "IN001035";
                       exid = "14";
                   }
                   else if (name == "ASE")
                   {
                       ccid = "IN001094";
                       exid = "15";
                   }
                   else if (name == "OTC")
                   {
                       ccid = "IN001086";
                       exid = "19";
                   }
                   else if (name == "NCD")
                   {
                       ccid = "IN001109";
                       exid = "22";
                   }
                   else if (name == "MCX")
                   {
                       ccid = "IN002322";
                       exid = "23";
                   }
                   else if (name == "BSL")
                   {
                       ccid = "IN001021";
                       exid = "25";
                   }
                   else if (name == "NSL")
                   {
                       ccid = "IN001020";
                       exid = "24";
                   }



                   CdslTransaction_InternalReference = des.Substring(33, 6);
                   CdslTransaction_CCID = ccid;
                   CdslTransaction_CounterBenID = des.Substring(43, 16);
                   CdslTransaction_ExchangeID = exid;

                   //  retValue = des.Substring(33, 6) + ",,," + ccid + ",," + des.Substring(43, 16) + ",," + exid + ",,,";

               }
               else

                   if (initial.Substring(0, 5).Equals("DEMAT"))
                   {
                       //str=initial.Split(' ');

                       CdslTransaction_InternalReference = des.Substring(6, 8);
                       CdslTransaction_DRNStatus = des.Substring(15);

                       //retValue = des.Substring(6, 8) + ",," + des.Substring(15) + ",,,,,,,,";

                   }
                   else if (initial.Substring(0, 5).Trim().Equals("OF-CR") || initial.Substring(0, 5).Trim().Equals("OF-DR"))
                   {
                       CdslTransaction_InternalReference = des.Substring(19, 6);
                       CdslTransaction_CounterBenID = des.Substring(26, 16);


                       // retValue = des.Substring(19, 6) + ",,,,," + des.Substring(26, 16) + ",,,,,";

                   }
                   else if (initial.Substring(0, 5).Trim().Equals("ON-CR") || initial.Substring(0, 5).Trim().Equals("ON-DR"))
                   {

                       int linelength = initial.Split(' ').Length;
                       CdslTransaction_InternalReference = des.Substring(19, 6);
                       CdslTransaction_CounterBenID = des.Substring(26, 16);
                       if (linelength == 5)
                       {
                           CdslTransaction_CounterSettlementId = des.Substring(47);
                       }

                       // retValue = des.Substring(19, 6) + ",,,,," + des.Substring(26, 16) + ",,,,,"+des.Substring(47);

                   }
                   else if ((initial.Substring(0, 5).Trim().Equals("BSECH") ||
                             initial.Substring(0, 5).Trim().Equals("NSCCL") ||
                               initial.Substring(0, 5).Trim().Equals("CSECH") ||
                                 initial.Substring(0, 5).Trim().Equals("DSECH") ||
                                     initial.Substring(0, 5).Trim().Equals("ASECH") ||
                                         initial.Substring(0, 5).Trim().Equals("OTCEI") ||
                                             initial.Substring(0, 5).Trim().Equals("NCDEX") ||
                                                 initial.Substring(0, 5).Trim().Equals("MCXCH") ||
                                                     initial.Substring(0, 5).Trim().Equals("BSSLB") ||
                                                    initial.Substring(0, 5).Trim().Equals("NSSLB")) && initial.Substring(5, 1).Equals("-"))
                   {


                       CdslTransaction_InternalReference = des.Substring(21, 12);
                       CdslTransaction_CCID = des.Substring(9, 8);
                       CdslTransaction_ExchangeID = des.Substring(18, 2);
                       if (des.Length > 39)
                           CdslTransaction_CounterSettlementId = des.Substring(39);

                       //  retValue = des.Substring(21, 12) + ",,," + des.Substring(9, 8) + ",,,," + des.Substring(18, 2) + ",,,"+des.Substring(39);

                   }
                   else if ((initial.Substring(0, 5).Trim().Equals("BSECH") ||
     initial.Substring(0, 5).Trim().Equals("NSCCL") ||
       initial.Substring(0, 5).Trim().Equals("CSECH") ||
         initial.Substring(0, 5).Trim().Equals("DSECH") ||
             initial.Substring(0, 5).Trim().Equals("ASECH") ||
                 initial.Substring(0, 5).Trim().Equals("OTCEI") ||
                     initial.Substring(0, 5).Trim().Equals("NCDEX") ||
                         initial.Substring(0, 5).Trim().Equals("MCXCH") ||
                             initial.Substring(0, 5).Trim().Equals("BSSLB") ||
                               initial.Substring(0, 5).Trim().Equals("NSSLB")) && (initial.Substring(6, 1).Equals("D") || initial.Substring(6, 1).Equals("C")))
                   {

                       string ccid = "", exid = "", name;
                       name = initial.Substring(0, 5);

                       if (name == "BSECH")
                       {
                           ccid = "IN001019";
                           exid = "11";
                       }
                       else if (name == "NSCCL")
                       {
                           ccid = "IN001002";
                           exid = "12";
                       }
                       else if (name == "CSECH")
                       {
                           ccid = "IN001027";
                           exid = "13";
                       }
                       else if (name == "DSECH")
                       {
                           ccid = "IN001035";
                           exid = "14";
                       }
                       else if (name == "ASECH")
                       {
                           ccid = "IN001094";
                           exid = "15";
                       }
                       else if (name == "OTCEI")
                       {
                           ccid = "IN001086";
                           exid = "19";
                       }
                       else if (name == "NCDEX")
                       {
                           ccid = "IN001109";
                           exid = "22";
                       }
                       else if (name == "MCXCH")
                       {
                           ccid = "IN002322";
                           exid = "23";
                       }
                       else if (name == "BSSLB")
                       {
                           ccid = "IN001021";
                           exid = "25";
                       }
                       else if (name == "NSSLB")
                       {
                           ccid = "IN001020";
                           exid = "24";
                       }


                       CdslTransaction_InternalReference = des.Substring(54, 6);
                       CdslTransaction_CCID = ccid;
                       CdslTransaction_CounterBenID = des.Substring(37, 16);
                       CdslTransaction_ExchangeID = exid;
                       CdslTransaction_CMID = des.Substring(28, 8);
                       CdslTransaction_CounterSettlementId = des.Substring(14, 10);

                       //   retValue = des.Substring(54, 6) + ",,," + ccid + ",," + des.Substring(37, 16) + ",," + exid + "," + des.Substring(28, 8) + ",," + des.Substring(14, 10);

                   }


                   else if (initial.Substring(0, 5).Trim().Equals("EP-DR") ||
                               initial.Substring(0, 5).Trim().Equals("EP-CR"))
                   {

                       CdslTransaction_InternalReference = des.Substring(10, 6);
                       CdslTransaction_CounterBenID = des.Substring(22, 16);
                       CdslTransaction_ExchangeID = des.Substring(58, 2);


                       //   retValue = des.Substring(10, 6) + ",,,,," + des.Substring(22, 16) + ",," + des.Substring(58, 2) + ",,,";

                   }

                   else if (initial.Substring(0, 5).Trim().Equals("EPRDR") ||
                               initial.Substring(0, 5).Trim().Equals("EPRCR"))
                   {

                       CdslTransaction_InternalReference = des.Substring(19, 6);
                       CdslTransaction_CounterBenID = des.Substring(26, 16);

                       //   retValue = des.Substring(19, 6) + ",,,,," + des.Substring(26, 16) + ",,,,,";

                   }



                   else
                       if (initial.Substring(0, 6).Equals("REMAT-"))
                       {
                           string rematRequest = initial.Substring(6, 7).Trim();

                           if (rematRequest == "CONFIRM")
                           {
                               flag = "7";
                           }
                           else if (rematRequest == "CANCEL")
                           {
                               flag = "6";
                           }
                           else if (rematRequest == "SETUP")
                           {
                               flag = "5";
                           }
                           else if (rematRequest == "REJECT")
                           {
                               flag = "6";
                           }

                           CdslTransaction_InternalReference = des.Substring(14, 8);
                           CdslTransaction_DRNStatus = flag;


                           // retValue = des.Substring(14, 8) + ",," + flag + ",,,,,,,,";

                       }
                       else if (initial.Substring(0, 7).Trim().Equals("CA-CA TYPE"))
                       {

                           CdslTransaction_SequenceNumber = des.Substring(26, 8);
                           CdslTransaction_DRNStatus = des.Substring(35);

                           // retValue = "," + des.Substring(26, 8) + "," + des.Substring(35) + ",,,,,,,,";

                       }
                       else if (initial.Substring(0, 8).Trim().Equals("EARMARK-"))
                       {
                           CdslTransaction_InternalReference = des.Substring(36, 6);
                           CdslTransaction_ExchangeID = des.Substring(33, 2);

                           // retValue = des.Substring(36, 6) + ",,,,,,," + des.Substring(33, 2) + ",,,";

                       }
                       else if (initial.Substring(0, 9).Trim().Equals("INTDEP-DR"))
                       {
                           string[] tmp = initial.Substring(27).Split(' ');
                           if (tmp[2].Length > 8)
                           {
                               CdslTransaction_InternalReference = des.Substring(10, 16);
                               CdslTransaction_CounterCMBPID = tmp[1];
                               CdslTransaction_CounterSettlementId = tmp[2];

                               //   retValue = des.Substring(10, 16) + ",,,," + tmp[1] + ",,,,,," + tmp[2];
                           }
                           else
                           {
                               CdslTransaction_InternalReference = des.Substring(10, 16);
                               CdslTransaction_CounterBenID = tmp[2];
                               CdslTransaction_CounterDPID = tmp[1];

                               // retValue = des.Substring(10, 16) + ",,,,," + tmp[2] + "," + tmp[1] + ",,,,";
                           }


                       }
                       else if (initial.Substring(0, 9).Trim().Equals("INTDEP-CR"))
                       {
                           string[] tmp = initial.Substring(27).Split(' ');
                           if (tmp.Length == 2)
                           {
                               CdslTransaction_InternalReference = des.Substring(10, 16);
                               CdslTransaction_CounterCMBPID = tmp[1];

                               // retValue = des.Substring(10, 16) + ",,,," +tmp[1]+ ",,,,,,";
                           }
                           else //if (tmp.Length == 3)
                           {
                               if (tmp[2].Length > 8)
                               {
                                   CdslTransaction_InternalReference = des.Substring(10, 16);
                                   CdslTransaction_CounterCMBPID = tmp[1];
                                   CdslTransaction_CounterSettlementId = tmp[2];

                                   //  retValue = des.Substring(10, 16) + ",,,," + tmp[1] + ",,,,,," + tmp[2];
                               }
                               else
                               {
                                   CdslTransaction_InternalReference = des.Substring(10, 16);
                                   CdslTransaction_CounterBenID = tmp[2];
                                   CdslTransaction_CounterDPID = tmp[1];

                                   //  retValue = des.Substring(10, 16) + ",,,,, "+ tmp[2]+"," + tmp[1] + ",,,,";
                               }

                           }




                       }
                       else if (initial.Substring(0, 10).Trim().Equals("SEC ELMN"))
                       {
                           CdslTransaction_SequenceNumber = des.Substring(11, 8);
                           CdslTransaction_DRNStatus = des.Substring(38);

                           // retValue = "," + des.Substring(11, 8) + "," + des.Substring(38) + ",,,,,,,,";

                       }
                       else if (initial.Substring(0, 10).Trim().Equals("A TRANSFER"))
                       {
                           CdslTransaction_InternalReference = des.Substring(16, 8);
                           CdslTransaction_CounterBenID = des.Substring(25, 16);

                           // retValue = des.Substring(16, 8) + ",,,,," + des.Substring(25, 16) + ",,,,,";

                       }
                       else if (initial.Substring(0, 10).Trim().Equals("OVERDUE-DR") ||
                              initial.Substring(0, 10).Trim().Equals("OVERDUE-CR"))
                       {

                           CdslTransaction_CounterBenID = des.Substring(16, 16);

                           //   retValue = ",,,,," + des.Substring(16, 16) + ",,,,,";

                       }
                       else if (initial.Substring(0, 11).Trim().Equals("AC TRANSFER"))
                       {
                           CdslTransaction_InternalReference = des.Substring(17, 8);
                           CdslTransaction_CounterBenID = des.Substring(26, 16);

                           //  retValue = des.Substring(17, 8) + ",,,,," + des.Substring(26, 16) + ",,,,,";

                       }
                       else if (initial.Substring(0, 11).Trim().Equals("BSEBOPAYIN"))
                       {
                           CdslTransaction_InternalReference = des.Substring(33, 6);
                           CdslTransaction_CounterBenID = des.Substring(43, 16);

                           //  retValue = des.Substring(33, 6) + ",,,,," + des.Substring(43, 16) + ",,,,,";

                       }
                       else if (initial.Substring(0, 11).Trim().Equals("BSE-PAYOUT-"))
                       {

                           CdslTransaction_CounterBenID = des.Substring(36, 16);

                           // retValue = ",,,,," + des.Substring(36, 16) + ",,,,,";

                       }
                       else if (initial.Substring(0, 11).Trim().Equals("CONVERT SP-"))
                       {
                           CdslTransaction_InternalReference = des.Substring(39, 6);
                           CdslTransaction_ExchangeID = des.Substring(33, 2);

                           //  retValue = des.Substring(39, 6) + ",,,,,,," + des.Substring(33, 2) + ",,,,";

                       }
                       else if (initial.Substring(0, 11).Trim().Equals("SHIFT BACK-"))
                       {
                           CdslTransaction_InternalReference = des.Substring(39, 6);
                           CdslTransaction_ExchangeID = des.Substring(33, 2);

                           //  retValue = des.Substring(39, 6) + ",,,,,,," + des.Substring(33, 2) + ",,,,";

                       }
                       else if (initial.Substring(0, 13).Trim().Equals("TRANSFER – DB"))
                       {
                           CdslTransaction_DRNStatus = des.Substring(14, 8);

                           //  retValue = ",," + des.Substring(14, 8) + ",,,,,,,,";

                       }
                       else if (initial.Substring(0, 13).Trim().Equals("TRANSFER – CR"))
                       {
                           CdslTransaction_CounterBenID = des.Substring(14, 16);

                           //  retValue = ",,,,," + des.Substring(14, 16) + ",,,,,";

                       }
                       else if (initial.Substring(0, 13).Trim().Equals("ID-EARMARK-CR"))
                       {
                           CdslTransaction_InternalReference = des.Substring(14);

                           //  retValue = des.Substring(14) + ",,,,,,,,,,";

                       }
                       else if (initial.Substring(0, 13).Trim().Equals("PLEDGE ACCEPT"))
                       {
                           CdslTransaction_SequenceNumber = des.Substring(18, 8);
                           CdslTransaction_CounterBenID = des.Substring(37, 16);

                           // retValue = "," + des.Substring(18, 8) + ",,,," + des.Substring(37, 16) + ",,,,,";

                       }
                       else if (initial.Substring(0, 13).Trim().Equals("SETTLEMENT-DR"))
                       {
                           CdslTransaction_InternalReference = des.Substring(39, 6);
                           CdslTransaction_CMID = des.Substring(36, 2);

                           //  retValue = des.Substring(39, 6) + ",,,,,,,," + des.Substring(36, 2) + ",,,";

                       }
                       else if (initial.Substring(0, 13).Trim().Equals("SETTLEMENT-CR"))
                       {

                           CdslTransaction_ExchangeID = des.Substring(36, 2);
                           CdslTransaction_CounterSettlementId = des.Substring(19, des.Substring(19).IndexOf(" "));

                           //  retValue = ",,,,,,," + des.Substring(36, 2) + ",,," + des.Substring(19, des.Substring(19).IndexOf(" "));

                       }
                       else if (initial.Substring(0, 15).Trim().Equals("UNPLEDGE ACCEPT"))
                       {
                           CdslTransaction_SequenceNumber = des.Substring(20, 8);
                           CdslTransaction_DRNStatus = des.Substring(42);

                           // retValue = "," + des.Substring(20, 8) + "," + des.Substring(42) + ",,,,,,,,,";

                       }
                       else if (initial.Substring(0, 17).Trim().Equals("INTDEP-EARMARK-DR"))
                       {
                           CdslTransaction_InternalReference = des.Substring(18);
                           //  retValue = des.Substring(18) + ",,,,,,,,,,";

                       }
                       else if (initial.Substring(0, 17).Trim().Equals("CONFISCATE ACCEPT"))
                       {
                           CdslTransaction_SequenceNumber = des.Substring(22, 8);
                           CdslTransaction_DRNStatus = des.Substring(44);

                           // retValue = "," + des.Substring(22, 8) + "," + des.Substring(44) + ",,,,,,,,";

                       }
                       else if (initial.Substring(0, 20).Trim().Equals("TRANSMISSION – DEBIT"))
                       {
                           CdslTransaction_CounterBenID = des.Substring(22, 16);

                           //  retValue = ",,,,," + des.Substring(22, 16) + ",,,,,";

                       }
                       else if (initial.Substring(0, 20).Trim().Equals("SAFEKEEP REMOVED ID:") && initial.Length == 60)
                       {
                           CdslTransaction_SequenceNumber = des.Substring(20, 8);
                           CdslTransaction_DRNStatus = des.Substring(52, 8);
                           CdslTransaction_FreezeExpdt = des.Substring(38, 10);

                           //   retValue = "," + des.Substring(20, 8) + "," + des.Substring(52, 8) + ",,,,,,," + des.Substring(38, 10)+",";

                       }
                       else if (initial.Substring(0, 20).Trim().Equals("SAFEKEEP REMOVED ID:") && initial.Length == 50)
                       {
                           CdslTransaction_SequenceNumber = des.Substring(20, 8);
                           CdslTransaction_DRNStatus = des.Substring(42);

                           //   retValue = "," + des.Substring(20, 8) + "," + des.Substring(42) + ",,,,,,,,";

                       }
                       else if (initial.Substring(0, 21).Trim().Equals("TRANSMISSION – CREDIT"))
                       {
                           CdslTransaction_CounterBenID = des.Substring(23, 16);

                           //  retValue = ",,,,," + des.Substring(23, 16) + ",,,,,";

                       }
                       else if (initial.Substring(0, 21).Trim().Equals("SAFEKEEP EXECUTED ID:") && des.Substring(53).Length == 7)
                       {
                           CdslTransaction_SequenceNumber = des.Substring(21, 8);
                           CdslTransaction_DRNStatus = des.Substring(53, 7);
                           CdslTransaction_FreezeExpdt = des.Substring(39, 10);

                           //  retValue = "," + des.Substring(21, 8) + "," + des.Substring(53, 7) + ",,,,,,," + des.Substring(39, 10)+",";

                       }
                       else if (initial.Substring(0, 21).Trim().Equals("SAFEKEEP EXECUTED ID:") && des.Substring(43).Length == 8)
                       {
                           CdslTransaction_SequenceNumber = des.Substring(21, 8);
                           CdslTransaction_DRNStatus = des.Substring(43);

                           //   retValue = "," + des.Substring(21, 8) + "," + des.Substring(43) + ",,,,,,,,";

                       }
                       else if (initial.Substring(0, 23).Equals("INITIAL PUBLIC OFFERING"))
                       {
                           CdslTransaction_InternalReference = des.Substring(35, 16);
                           CdslTransaction_SequenceNumber = des.Substring(26, 8);
                           CdslTransaction_DRNStatus = "CREDIT";


                           // retValue = des.Substring(35, 16) + "," + des.Substring(26, 8) + ",CREDIT,,,,,,,,";
                       }
                       else if (initial.Substring(0, 24).Trim().Equals("CH ESC BO RELEASE CREDIT"))
                       {

                           CdslTransaction_CounterBenID = des.Substring(25, 16);

                           // retValue = ",,,,," + des.Substring(25, 16) + ",,,,,";

                       }
                       else if (initial.Substring(0, 24).Trim().Equals("CH ESC BO RELEASE DEBIT"))
                       {
                           CdslTransaction_CounterBenID = des.Substring(24, 16);

                           //   retValue = ",,,,," + des.Substring(24, 16) + ",,,,,";

                       }

                       else if (initial.Substring(0, 25).Trim().Equals("CA- TYPE"))
                       {

                           CdslTransaction_InternalReference = des.Substring(35, 16);
                           CdslTransaction_SequenceNumber = des.Substring(26, 8);
                           CdslTransaction_DRNStatus = des.Substring(52);

                           //retValue = des.Substring(35, 16) + "," + des.Substring(26, 8) + "," + des.Substring(52) + ",,,,,,,,";
                       }
                       else if (initial.Substring(0, 25).Trim().Equals("CORPORATE ACTION TYPE"))
                       {
                           CdslTransaction_SequenceNumber = des.Substring(26, 8);
                           CdslTransaction_DRNStatus = des.Substring(38);

                           // retValue = "," + des.Substring(26, 8) + "," + des.Substring(38) + ",,,,,,,,";

                       }
                       else if (initial.Substring(0, 25).Trim().Equals("CH ESCROW RELEASE – DEBIT"))
                       {
                           CdslTransaction_CounterBenID = des.Substring(27, 16);

                           //  retValue = ",,,,," + des.Substring(27, 16) + ",,,,,";

                       }

                       else if (initial.Substring(0, 26).Trim().Equals("CH ESCROW RELEASE – CREDIT"))
                       {
                           CdslTransaction_CounterBenID = des.Substring(28, 16);

                           //  retValue = ",,,,," + des.Substring(28, 16) + ",,,,,";

                       }
                       else
                       {
                           nonMatchDescription = "Not Parse: " + des;
                       }

           }
           catch
           {
               nonParseTransasction = "New Description Format: " + des;
           }

           if (nonMatchDescription != String.Empty)
           {

               retValue = nonMatchDescription;
           }
           else
               if (nonParseTransasction != String.Empty)
               {
                   retValue = nonParseTransasction;

               }
               else
               {
                   retValue = CdslTransaction_InternalReference + "," +
                               CdslTransaction_SequenceNumber + "," +
                               CdslTransaction_DRNStatus + "," +
                               CdslTransaction_CCID + "," +
                               CdslTransaction_CounterCMBPID + "," +
                               CdslTransaction_CounterBenID + "," +
                               CdslTransaction_CounterDPID + "," +
                               CdslTransaction_ExchangeID + "," +
                               CdslTransaction_CMID + "," +
                               CdslTransaction_FreezeExpdt + "," +
                               CdslTransaction_CounterSettlementId;

               }



           return retValue;


       } 
    }
}
