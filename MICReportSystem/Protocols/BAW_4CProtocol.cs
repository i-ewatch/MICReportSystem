using NModbus;
using Serilog;
using System;
using System.Threading;

namespace MICReportSystem.Protocols
{
    public class BAW_4CProtocol : ThreePhaseElectricMeterData
    {
        public override void ReadData(IModbusMaster master)
        {
            try
            {
                ushort[] HZ = master.ReadHoldingRegisters(ID, 304, 1);
                ushort[] V = master.ReadHoldingRegisters(ID, 305, 6);
                ushort[] A = master.ReadHoldingRegisters(ID, 312, 6);
                ushort[] data = master.ReadHoldingRegisters(ID, 318, 16);
                ushort[] KWH = master.ReadHoldingRegisters(ID, 40960, 2);
                ushort[] KVARH = master.ReadHoldingRegisters(ID, 40990, 2);
                int Index = 0;
                hz = Convert.ToDecimal(HZ[0] * 0.01);
                rsv = Convert.ToDecimal(MathClass.work16to10(V[Index], V[Index + 1]) * 0.01); Index += 2;
                stv = Convert.ToDecimal(MathClass.work16to10(V[Index], V[Index + 1]) * 0.01); Index += 2;
                trv = Convert.ToDecimal(MathClass.work16to10(V[Index], V[Index + 1]) * 0.01);
                Index = 0;
                ra = Convert.ToDecimal(MathClass.work16to10(A[Index], A[Index + 1]) * 0.01); Index += 2;
                sa = Convert.ToDecimal(MathClass.work16to10(A[Index], A[Index + 1]) * 0.01); Index += 2;
                ta = Convert.ToDecimal(MathClass.work16to10(A[Index], A[Index + 1]) * 0.01);
                Index = 0;
                _ = Convert.ToDecimal(data[Index] * 0.01); Index++;//P1
                _ = Convert.ToDecimal(data[Index] * 0.01); Index++;//P2
                _ = Convert.ToDecimal(data[Index] * 0.01); Index++;//P3
                kw = Convert.ToDecimal(data[Index] * 0.01*(ReportConfig.Ratio)); Index++;
                _ = Convert.ToDecimal(data[Index] * 0.01); Index++;//Q1
                _ = Convert.ToDecimal(data[Index] * 0.01); Index++;//Q2
                _ = Convert.ToDecimal(data[Index] * 0.01); Index++;//Q3
                kvar = Convert.ToDecimal(data[Index] * 0.01); Index++;
                _ = Convert.ToDecimal(data[Index] * 0.01); Index++;//A1
                _ = Convert.ToDecimal(data[Index] * 0.01); Index++;//A2
                _ = Convert.ToDecimal(data[Index] * 0.01); Index++;//A3
                kva = Convert.ToDecimal(data[Index] * 0.01); Index++;
                _ = Convert.ToDecimal(data[Index] * 0.001); Index++;//PFE1
                _ = Convert.ToDecimal(data[Index] * 0.001); Index++;//PFE2
                _ = Convert.ToDecimal(data[Index] * 0.001); Index++;//PFE3
                pfe = Convert.ToDecimal(data[Index] * 0.001);
                Index = 0;
                kwh = Convert.ToDecimal(MathClass.work16to10(KWH[Index], KWH[Index + 1]) * 0.01 * (ReportConfig.Ratio));
                kvarh = Convert.ToDecimal(MathClass.work16to10(KVARH[Index], KVARH[Index + 1]) * 0.01);
                ConnectFlag = true;
                InsertSql();
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                Log.Error(ex, $"BAW-4C電表通訊失敗 ID : {ID}");
                ConnectFlag = false;
            }
        }
        public void InsertSql()
        {
            if (ConnectFlag)
            {
                MysqlMethod.Insert_ThreePhaseElectricMeter_Log(this);
            }
        }
    }
}
