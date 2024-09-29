/*
This is free and unencumbered software released into the public domain.
Anyone is free to copy, modify, publish, use, compile, sell, or distribute
this software, either in source code form or as a compiled binary, for any
purpose, commercial or non-commercial, and by any means.
In jurisdictions that recognize copyright laws, the author or authors of
this software dedicate any and all copyright interest in the software to
the public domain. We make this dedication for the benefit of the public at
large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of relinquishment
in perpetuity of all present and future rights to this software under
copyright law.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
For more information, please refer to <http://unlicense.org/>
*/
using AxKHOpenAPILib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
namespace Opt10081DBGenerator
{
    public class KOAPI : IDisposable
    {
        private class __2024_0002 : AxKHOpenAPI
        {
            public __2024_0002()
            {
                new Control().Controls.Add(this); //If this line is missing, System.Windows.Forms.AxHost+InvalidActiveXStateException will be thrown.
                EndInit(); //If this line is missing, System.Windows.Forms.AxHost+InvalidActiveXStateException will be thrown.
            }
        }
        private __2024_0002 ocx;
        public KOAPI()
        {
            //
            // ocx
            //
            Exception exception = null;
            using (AutoResetEvent autoResetEvent = new AutoResetEvent(false))
            {
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        Application.CurrentCulture = new CultureInfo("ko-KR");
                        ocx = new __2024_0002();
                        autoResetEvent.Set();
                        try
                        {
                            Application.Run();
                        }
                        catch (ThreadAbortException)
                        {
                        }
                    }
                    catch (Exception e)
                    {
                        exception = e;
                        autoResetEvent.Set();
                    }
                });
                thread.IsBackground = true;
                thread.CurrentUICulture = new CultureInfo("ko-KR");
                thread.CurrentCulture = new CultureInfo("ko-KR");
                thread.Name = "axkh";
                thread.SetApartmentState(ApartmentState.STA);//If this line is missing, System.Threading.ThreadStateException will be thrown.
                thread.Start();
                autoResetEvent.WaitOne();
            }
            if (exception != null)
                throw exception;
            if (ocx == null)
                throw new Exception();
            //
            // events
            //
            ocx.OnReceiveTrData += Ocx_OnReceiveTrData;
            ocx.OnReceiveMsg += Ocx_OnReceiveMsg;
            ocx.OnEventConnect += Ocx_OnEventConnect;
            ocx.OnReceiveInvestRealData += Ocx_OnReceiveInvestRealData;
            ocx.OnReceiveRealCondition += Ocx_OnReceiveRealCondition;
            ocx.OnReceiveTrCondition += Ocx_OnReceiveTrCondition;
            ocx.OnReceiveConditionVer += Ocx_OnReceiveConditionVer;
        }

        private static void Error(object x)
        {
            Console.WriteLine(x.ToString());
        }

        private static void Info(object x)
        {
            Console.WriteLine(x.ToString());
        }


        public event _DKHOpenAPIEvents_OnReceiveConditionVerEventHandler OnReceiveConditionVer;
        private void Ocx_OnReceiveConditionVer(object sender, _DKHOpenAPIEvents_OnReceiveConditionVerEvent e)
        {
            try
            {
                OnReceiveConditionVer?.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }


        public event _DKHOpenAPIEvents_OnReceiveTrConditionEventHandler OnReceiveTrCondition;
        private void Ocx_OnReceiveTrCondition(object sender, _DKHOpenAPIEvents_OnReceiveTrConditionEvent e)
        {
            try
            {
                OnReceiveTrCondition?.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }

        public event _DKHOpenAPIEvents_OnReceiveRealConditionEventHandler OnReceiveRealCondition;
        private void Ocx_OnReceiveRealCondition(object sender, _DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {
            try
            {
                OnReceiveRealCondition?.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }

        public event _DKHOpenAPIEvents_OnReceiveInvestRealDataEventHandler OnReceiveInvestRealData;
        private void Ocx_OnReceiveInvestRealData(object sender, _DKHOpenAPIEvents_OnReceiveInvestRealDataEvent e)
        {
            try
            {
                OnReceiveInvestRealData?.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }
        
        public event _DKHOpenAPIEvents_OnEventConnectEventHandler OnEventConnect;
        private readonly char[] m_separator = new char[] { ';' };
        private static readonly StringDictionary m_masterCodeName = new StringDictionary();
        private static readonly StringDictionary m_masterLastPrice = new StringDictionary();
        private void Ocx_OnEventConnect(object sender, _DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            try
            {
                if (e.nErrCode == 0)
                {
                    string[] codeList = GetCodeListByMarket(null);
                    foreach (var code in codeList)
                    {
                        m_masterCodeName[code] = ocx.GetMasterCodeName(code);
                        m_masterLastPrice[code] = ocx.GetMasterLastPrice(code);
                    }
                    if (IsTestServerConnected)
                    {
                        Info("Connected to test server.");
                    }
                    else
                    {
                        Info("Connected to real server.");
                    }
                    m_commConnectSyncARE.Set();
                }
                else
                {
                    Error($"OnEventConnect error code is {e.nErrCode}.");
                }
                OnEventConnect?.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }

        public event _DKHOpenAPIEvents_OnReceiveMsgEventHandler OnReceiveMsg;
        private void Ocx_OnReceiveMsg(object sender, _DKHOpenAPIEvents_OnReceiveMsgEvent e)
        {
            try
            {
                if (e != null)
                    Info($"OnReceiveMsg({e.sMsg},{e.sRQName},{e.sTrCode},{e.sScrNo})");
                OnReceiveMsg?.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }

        public int m_onReceiveTrDataCount = 0;
        public event _DKHOpenAPIEvents_OnReceiveTrDataEventHandler OnReceiveTrData;
        private void Ocx_OnReceiveTrData(object sender, _DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            try
            {
                if (e != null)
                {
                    m_onReceiveTrDataCount++;
                    Info(
$"#{m_onReceiveTrDataCount} OnReceiveTrData(" +
$"{e.sTrCode},{e.sPrevNext},{e.sRecordName},{e.sRQName},{e.nDataLength},{e.sErrorCode},{e.sMessage},{e.sScrNo},{e.sSplmMsg})");
                }

                m_commRqDataSync_sPrevNext = e.sPrevNext;
                m_commRqDataSync_handler?.Invoke(sender, e);
                m_commRqDataSync_are.Set();

                OnReceiveTrData?.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }

        //
        // CommRqDataSync
        //
        private _DKHOpenAPIEvents_OnReceiveTrDataEventHandler m_commRqDataSync_handler;
        private readonly ManualResetEvent m_commRqDataSync_are = new ManualResetEvent(false);
        private string m_commRqDataSync_sPrevNext;
        /// <summary>
        /// CommRqData and wait up to 10 seconds to invoke handler. Return after the handler is invoked.
        /// </summary>
        /// <exception cref="TimeoutException"/>
        /// <exception cref="System.Runtime.InteropServices.InvalidComObjectException"/>
        public ReturnAndPrevNext CommRqDataSync(
            string sRQName, string sTrCode, int nPrevNext, string sScreenNo, Action<int> returnCallback, _DKHOpenAPIEvents_OnReceiveTrDataEventHandler handler)
        {
            m_commRqDataSync_handler = handler;
            m_commRqDataSync_sPrevNext = null;
            m_commRqDataSync_are.Reset();
            int ret = CommRqData(sRQName, sTrCode, nPrevNext, sScreenNo);
            returnCallback?.Invoke(ret);
            if (ret == 0)
            {
                bool flag = m_commRqDataSync_are.WaitOne(10000);//10s
                if (!flag) throw new TimeoutException("CommRqDataSyncTimeout");
            }
            Thread.MemoryBarrier();
            return new ReturnAndPrevNext(ret, m_commRqDataSync_sPrevNext);
        }

        public virtual int CommConnect()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("CommConnect", ActiveXInvokeKind.MethodInvoke);
            }

            int ret = ocx.CommConnect();
            Info("CommConnect()=" + ret);
            return ret;
        }

        private readonly AutoResetEvent m_commConnectSyncARE = new AutoResetEvent(false);
        public int CommConnectSync()
        {
            int ret = CommConnect();
            m_commConnectSyncARE.WaitOne();
            return ret;
        }

        public virtual void CommTerminate()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("CommTerminate", ActiveXInvokeKind.MethodInvoke);
            }

            ocx.CommTerminate();
        }

        /// <exception cref="System.Runtime.InteropServices.InvalidComObjectException"/>
        public virtual int CommRqData(string sRQName, string sTrCode, int nPrevNext, string sScreenNo)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("CommRqData", ActiveXInvokeKind.MethodInvoke);
            }

                int ret = ocx.CommRqData(sRQName, sTrCode, nPrevNext, sScreenNo);
                return ret;
        }

        public virtual string GetLoginInfo(string sTag)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetLoginInfo", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetLoginInfo(sTag);
        }

        public virtual int SendOrder(string sRQName, string sScreenNo, string sAccNo, int nOrderType, string sCode, int nQty, int nPrice, string sHogaGb, string sOrgOrderNo)
        {
            throw new NotImplementedException();
        }

        public virtual int SendOrderFO(string sRQName, string sScreenNo, string sAccNo, string sCode, int lOrdKind, string sSlbyTp, string sOrdTp, int lQty, string sPrice, string sOrgOrdNo)
        {
            throw new NotImplementedException();
        }

        public virtual void SetInputValue(string sID, string sValue)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("SetInputValue", ActiveXInvokeKind.MethodInvoke);
            }

            ocx.SetInputValue(sID, sValue);
        }

        public virtual int SetOutputFID(string sID)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("SetOutputFID", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.SetOutputFID(sID);
        }

        public virtual string CommGetData(string sJongmokCode, string sRealType, string sFieldName, int nIndex, string sInnerFieldName)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("CommGetData", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.CommGetData(sJongmokCode, sRealType, sFieldName, nIndex, sInnerFieldName);
        }

        public virtual void DisconnectRealData(string sScnNo)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("DisconnectRealData", ActiveXInvokeKind.MethodInvoke);
            }

            ocx.DisconnectRealData(sScnNo);
        }

        public virtual int GetRepeatCnt(string sTrCode, string sRecordName)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetRepeatCnt", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetRepeatCnt(sTrCode, sRecordName);
        }

        private int m_commKwRqDataCount;
        private int CommKwRqData(string sArrCode, int bNext, int nCodeCount, int nTypeFlag, string sRQName, string sScreenNo)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("CommKwRqData", ActiveXInvokeKind.MethodInvoke);
            }

            int ret = ocx.CommKwRqData(sArrCode, bNext, nCodeCount, nTypeFlag, sRQName, sScreenNo);
            m_commKwRqDataCount++;
            Info($"#{m_commKwRqDataCount} CommKwRqData({sArrCode},{bNext},{nCodeCount},{nTypeFlag},{sRQName},{sScreenNo})={ret}");
            return ret;
        }

        public virtual string GetAPIModulePath()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetAPIModulePath", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetAPIModulePath();
        }

        /// <summary>
        ///[GetCodeListByMarket2() 함수]
        ///GetCodeListByMarket2(
        ///BSTR sMarket    // 시장구분값
        ///)
        ///
        ///
        ///주식 시장별 종목코드 리스트를 전달합니다.
        ///시장구분값을 ""공백으로하면 전체시장 코드리스트를 전달합니다.
        ///
        ///로그인 한 후에 사용할 수 있는 함수입니다.
        ///
        ///
        ///[시장구분값]
        ///0 : 코스피
        ///10 : 코스닥
        ///3 : ELW
        ///8 : ETF
        ///50 : KONEX
        ///4 :  뮤추얼펀드
        ///5 : 신주인수권
        ///6 : 리츠
        ///9 : 하이얼펀드
        ///30 : K-OTC
        /// </summary>
        /// <param name="sMarket">
        ///[시장구분값]
        ///0 : 코스피
        ///10 : 코스닥
        ///3 : ELW
        ///8 : ETF
        ///50 : KONEX
        ///4 :  뮤추얼펀드
        ///5 : 신주인수권
        ///6 : 리츠
        ///9 : 하이얼펀드
        ///30 : K-OTC
        ///시장구분값을 ""공백으로하면 전체시장 코드리스트를 전달합니다.
        /// </param>
        /// <returns>주식 시장별 종목코드 리스트</returns>
        public string[] GetCodeListByMarket(string sMarket)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetCodeListByMarket", ActiveXInvokeKind.MethodInvoke);
            }

            string sRet = ocx.GetCodeListByMarket(sMarket);
            return sRet.Split(m_separator, StringSplitOptions.RemoveEmptyEntries);
        }

        private int GetConnectState()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetConnectState", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetConnectState();
        }

        public virtual string GetMasterCodeName(string sTrCode)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetMasterCodeName", ActiveXInvokeKind.MethodInvoke);
            }

            return m_masterCodeName[sTrCode];
        }

        public virtual int GetMasterListedStockCnt(string sTrCode)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetMasterListedStockCnt", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetMasterListedStockCnt(sTrCode);
        }

        public virtual string GetMasterConstruction(string sTrCode)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetMasterConstruction", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetMasterConstruction(sTrCode);
        }

        public virtual string GetMasterListedStockDate(string sTrCode)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetMasterListedStockDate", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetMasterListedStockDate(sTrCode);
        }

        public virtual int GetMasterLastPrice(string sTrCode)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetMasterLastPrice", ActiveXInvokeKind.MethodInvoke);
            }

            if (string.IsNullOrWhiteSpace(sTrCode)) return 0;
            string sRet = m_masterLastPrice[sTrCode];
            int.TryParse(sRet, out int result);
            return result;
        }

        public virtual string GetMasterStockState(string sTrCode)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetMasterStockState", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetMasterStockState(sTrCode);
        }

        public virtual int GetDataCount(string strRecordName)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetDataCount", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetDataCount(strRecordName);
        }

        public virtual string GetOutputValue(string strRecordName, int nRepeatIdx, int nItemIdx)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetOutputValue", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetOutputValue(strRecordName, nRepeatIdx, nItemIdx);
        }

        public virtual string GetCommData(string strTrCode, string strRecordName, int nIndex, string strItemName)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetCommData", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetCommData(strTrCode, strRecordName, nIndex, strItemName);
        }

        public virtual string GetCommRealData(string sTrCode, int nFid)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetCommRealData", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetCommRealData(sTrCode, nFid);
        }

        public virtual string GetChejanData(int nFid)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetChejanData", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetChejanData(nFid);
        }

        public virtual string GetThemeGroupList(int nType)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetThemeGroupList", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetThemeGroupList(nType);
        }

        public virtual string GetThemeGroupCode(string strThemeCode)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetThemeGroupCode", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetThemeGroupCode(strThemeCode);
        }

        public virtual string GetFutureList()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetFutureList", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetFutureList();
        }

        public virtual string GetFutureCodeByIndex(int nIndex)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetFutureCodeByIndex", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetFutureCodeByIndex(nIndex);
        }

        public virtual string GetActPriceList()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetActPriceList", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetActPriceList();
        }

        public virtual string GetMonthList()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetMonthList", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetMonthList();
        }

        public virtual string GetOptionCode(string strActPrice, int nCp, string strMonth)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetOptionCode", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetOptionCode(strActPrice, nCp, strMonth);
        }

        public virtual string GetOptionCodeByMonth(string sTrCode, int nCp, string strMonth)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetOptionCodeByMonth", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetOptionCodeByMonth(sTrCode, nCp, strMonth);
        }

        public virtual string GetOptionCodeByActPrice(string sTrCode, int nCp, int nTick)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetOptionCodeByActPrice", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetOptionCodeByActPrice(sTrCode, nCp, nTick);
        }

        public virtual string GetSFutureList(string strBaseAssetCode)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetSFutureList", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetSFutureList(strBaseAssetCode);
        }

        public virtual string GetSFutureCodeByIndex(string strBaseAssetCode, int nIndex)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetSFutureCodeByIndex", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetSFutureCodeByIndex(strBaseAssetCode, nIndex);
        }

        public virtual string GetSActPriceList(string strBaseAssetGb)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetSActPriceList", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetSActPriceList(strBaseAssetGb);
        }

        public virtual string GetSMonthList(string strBaseAssetGb)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetSMonthList", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetSMonthList(strBaseAssetGb);
        }

        public virtual string GetSOptionCode(string strBaseAssetGb, string strActPrice, int nCp, string strMonth)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetSOptionCode", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetSOptionCode(strBaseAssetGb, strActPrice, nCp, strMonth);
        }

        public virtual string GetSOptionCodeByMonth(string strBaseAssetGb, string sTrCode, int nCp, string strMonth)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetSOptionCodeByMonth", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetSOptionCodeByMonth(strBaseAssetGb, sTrCode, nCp, strMonth);
        }

        public virtual string GetSOptionCodeByActPrice(string strBaseAssetGb, string sTrCode, int nCp, int nTick)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetSOptionCodeByActPrice", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetSOptionCodeByActPrice(strBaseAssetGb, sTrCode, nCp, nTick);
        }

        public virtual string GetSFOBasisAssetList()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetSFOBasisAssetList", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetSFOBasisAssetList();
        }

        public virtual string GetOptionATM()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetOptionATM", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetOptionATM();
        }

        public virtual string GetSOptionATM(string strBaseAssetGb)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetSOptionATM", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetSOptionATM(strBaseAssetGb);
        }

        public virtual string GetBranchCodeName()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetBranchCodeName", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetBranchCodeName();
        }

        public virtual int CommInvestRqData(string sMarketGb, string sRQName, string sScreenNo)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("CommInvestRqData", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.CommInvestRqData(sMarketGb, sRQName, sScreenNo);
        }

        public virtual int SendOrderCredit(string sRQName, string sScreenNo, string sAccNo, int nOrderType, string sCode, int nQty, int nPrice, string sHogaGb, string sCreditGb, string sLoanDate, string sOrgOrderNo)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("SendOrderCredit", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.SendOrderCredit(sRQName, sScreenNo, sAccNo, nOrderType, sCode, nQty, nPrice, sHogaGb, sCreditGb, sLoanDate, sOrgOrderNo);
        }

        public virtual string KOA_Functions(string sFunctionName, string sParam)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("KOA_Functions", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.KOA_Functions(sFunctionName, sParam);
        }

        public virtual int SetInfoData(string sInfoData)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("SetInfoData", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.SetInfoData(sInfoData);
        }

        public virtual int SetRealReg(string strScreenNo, string strCodeList, string strFidList, string strOptType)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("SetRealReg", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.SetRealReg(strScreenNo, strCodeList, strFidList, strOptType);
        }

        public virtual int GetConditionLoad()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetConditionLoad", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetConditionLoad();
        }

        public virtual string GetConditionNameList()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetConditionNameList", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetConditionNameList();
        }

        public virtual int SendCondition(string strScrNo, string strConditionName, int nIndex, int nSearch)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("SendCondition", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.SendCondition(strScrNo, strConditionName, nIndex, nSearch);
        }

        public virtual void SendConditionStop(string strScrNo, string strConditionName, int nIndex)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("SendConditionStop", ActiveXInvokeKind.MethodInvoke);
            }

            ocx.SendConditionStop(strScrNo, strConditionName, nIndex);
        }

        public virtual object GetCommDataEx(string strTrCode, string strRecordName)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetCommDataEx", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetCommDataEx(strTrCode, strRecordName);
        }

        public virtual void SetRealRemove(string strScrNo, string strDelCode)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("SetRealRemove", ActiveXInvokeKind.MethodInvoke);
            }

            ocx.SetRealRemove(strScrNo, strDelCode);
        }

        public virtual int GetMarketType(string sTrCode)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetMarketType", ActiveXInvokeKind.MethodInvoke);
            }

            return ocx.GetMarketType(sTrCode);
        }

        public void Dispose()
        {
            ocx?.Dispose();
        }

        public bool IsDisposed => ocx?.IsDisposed ?? true;

        private int m_NewScrNo = 0;
        /// <summary>
        /// 1000 이상 1009 이하의 화면 번호를 오름차순으로 생성합니다. 1009 다음에는 1000이 옵니다.
        /// </summary>
        /// <returns>1000 이상 1009 이하의 화면 번호</returns>
        public string NewScrNo()
        {
            if (m_NewScrNo >= 10) { m_NewScrNo = 0; }
            string ret = (1000 + m_NewScrNo).ToString();
            m_NewScrNo++;
            return ret;
        }

        private int m_NewRQNameIndex = 0;

        /// <summary>
        /// RQ_1 이상 RQ_2147483647 이하의 사용자 지정명을 오름차순으로 생성합니다.
        /// </summary>
        /// <returns>RQ_1 이상 RQ_2147483647 이하의 화면 번호</returns>
        /// <exception cref="OverflowException"/>
        public string NewRQName()
        {
            if (m_NewRQNameIndex == int.MaxValue)
            {
                throw new OverflowException();
            }
            m_NewRQNameIndex++;
            return "RQ_" + m_NewRQNameIndex;
        }

        ///<returns>자동 로그인 정보가 담겨있는 파일의 경로를 반환합니다.</returns>
        public string Autologin_dat => System.IO.Path.Combine(GetAPIModulePath(), "system", "Autologin.dat");

        /// <summary>
        /// 해당 종목코드를 가진 종목이 존재하는지의 여부를 확인합니다.
        /// </summary>
        public bool JmcodeExists(string jmcode)
        {
            if (string.IsNullOrWhiteSpace(jmcode))
            {
                return false;
            }
            jmcode = jmcode.Trim();
            string name = GetMasterCodeName(jmcode);
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            int price = GetMasterLastPrice(jmcode);
            if (price <= 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 10. 종목코드로 Market구분 구하기 (2022/3/3 적용)
        /// 종목코드 입력으로 해당 종목이 어느 시장에 포함되어 있는지 구하는 기능
        /// 서버와의 통신없이 메모리에 상주하는 값을 사용하므로 횟수제한 등은 없습니다.사용법은 아래와 같습니다.
        /// 
        /// KOA_Functions("GetStockMarketKind", "종목코드6자리");
        /// 리턴값은 문자형으로 아래와 같습니다.
        ///  "0":코스피, "10":코스닥, "3":ELW, "8":ETF, "4"/"14":뮤추얼펀드, "6"/"16":리츠, "9"/"19":하이일드펀드, "30":제3시장, "60":ETN
        /// </summary>
        /// <param name="jmcode">종목코드 6자리</param>
        /// <returns>"0":코스피, "10":코스닥, "3":ELW, "8":ETF, "4"/"14":뮤추얼펀드, "6"/"16":리츠, "9"/"19":하이일드펀드, "30":제3시장, "60":ETN</returns>
        public string GetStockMarketKind(string jmcode)
        {
            return KOA_Functions("GetStockMarketKind", jmcode);
        }

        /// <summary>
        ///[bool ConnectState]
        ///
        ///서버와 현재 접속 상태를 알려줍니다.
        ///리턴값 true:연결, false:연결안됨
        /// </summary>
        public bool ConnectState
        {
            get
            {
                try
                {
                    return GetConnectState() == 1;
                }
                catch (System.Runtime.InteropServices.InvalidComObjectException)
                {
                    return false;
                }
            }
        }

        /// <returns>10자리 계좌번호 리스트를 반환합니다.</returns>
        public string[] ACCLIST
        {
            get
            {
                return GetLoginInfo("ACCNO").Split(m_separator, StringSplitOptions.RemoveEmptyEntries);
            }
        }



        /// <returns>모의투자 서버에 연결되어 있다면 true, 아니면 false를 반환합니다.</returns>
        public bool IsTestServerConnected
        {
            get
            {
                return GetLoginInfo("GetServerGubun") == "1";
            }
        }

        /// <summary>
        /// 종목코드로 Market구분 구하기
        /// </summary>
        /// <param name="jmcode">종목코드 6자리</param>
        public Market GetMarket(string jmcode)
        {
            if (jmcode == null)
            {
                return Market.KOSPI;
            }
            string s = GetStockMarketKind(jmcode);
            switch (s)
            {
                case "0":
                    return Market.KOSPI;
                case "10":
                    return Market.KOSDAQ;
                case "3":
                    return Market.ELW;
                case "8":
                    return Market.ETF;
                case "4":
                case "14":
                    return Market.MutualFund;
                case "6":
                case "16":
                    return Market.Ritz;
                case "9":
                case "19":
                    return Market.HighYieldFund;
                case "30":
                    return Market.ThirdMarket;
                case "60":
                    return Market.ETN;

                //가이드에 없는 것들
                case "2"://맥쿼리인프라(088980)
                    return Market.KOSPI;
                case "-1"://존재하지 않는 종목
                    return Market.KOSPI;

                default:
                    return Market.KOSPI;
            }
        }

        public HashSet<string> GetHashSetByMarket(string market)
        {
            return GetCodeListByMarket(market).ToHashSet();
        }

        /// <summary>
        /// 모든 평범한 종목코드들을 반환합니다.
        /// 다음 조건들을 모두 만족시키면 평범합니다.
        /// 1. KOSPI 또는 KOSDAQ
        /// 2. SPAC, 우선주가 아님
        /// 3. 종목코드 끝자리가 0
        /// 4. 종목명이 존재
        /// 5. 당일기준가가 0보다 큰 정수
        /// 6. 종목코드의 길이가 6
        /// </summary>
        public HashSet<string> GetCommonCodes()
        {
            HashSet<string> ret = GetHashSetByMarket("0");//KOSPI
            ret.UnionWith(GetHashSetByMarket("10"));//KOSDAQ
            ret.ExceptWith(GetHashSetByMarket("3"));//ELW
            ret.ExceptWith(GetHashSetByMarket("8"));//ETF
            ret.ExceptWith(GetHashSetByMarket("50"));//KONEX
            ret.ExceptWith(GetHashSetByMarket("4"));//뮤추얼펀드
            ret.ExceptWith(GetHashSetByMarket("5"));//신주인수권
            ret.ExceptWith(GetHashSetByMarket("6"));//리츠
            ret.ExceptWith(GetHashSetByMarket("9"));//하이얼펀드
            ret.ExceptWith(GetHashSetByMarket("30"));//K-OTC
            ret.RemoveWhere(x => GetMasterCodeName(x).Contains("스팩"));
            ret.RemoveWhere(x => x.Length != 6);
            ret.RemoveWhere(x => x[5] != '0');
            ret.RemoveWhere(x => !JmcodeExists(x));
            ret.RemoveWhere(x =>
            {
                string s = GetStockMarketKind(x);
                return s != "0" && s != "10";
            });
            return ret;
        }

        private Opt10081Row[] __2024_0001(object sender, _DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            object commdataex = GetCommDataEx(e.sTrCode, e.sRecordName);
            object[,] commdataex2 = (object[,])commdataex;
            return Opt10081Row.FromDataEx2(commdataex2);
        }

        public Opt10081Row[] GetOpt10081Rows(string jmcode)
        {
            SetInputValue("종목코드",jmcode);
            Opt10081Row[] ret = null;
            AutoResetEvent are = new AutoResetEvent(false);
            CommRqDataSync(NewRQName(), "OPT10081", 0, NewScrNo(), null, (o, e) =>
            {
                ret = __2024_0001(o, e);
                are.Set();
            });
            are.WaitOne(0x3f3f3f3f);
            return ret;
        }

        public string GetCodeListByMarketRaw(string sMarket)
        {
            return ocx.GetCodeListByMarket(sMarket);
        }

        public string GetMasterLastPriceRaw(string sTrCode)
        {
            return ocx.GetMasterLastPrice(sTrCode);
        }

        public string[] GetLast600MarketOpenDates()
        {
            string[] ret = GetOpt10081Rows("005930").Select(x => x.Date).ToArray();
            if(ret.Length != 600)
            {
                throw new InvalidOperationException($"Expected array length 600, but actual {ret.Length}");
            }
            return ret;
        }

    }
}
