using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TAP;
using TAP.Data.DataBase.Communicators;
using TAP.Remoting;

namespace ISIA.BIZ.MANAGEMENT
{
    class CreateProceduresManagement : TAP.Remoting.Server.Biz.BizComponentBase
    {


        public void GetProcedures(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("SELECT * FROM USER_PROCEDURES WHERE OBJECT_NAME = '{0}'", arguments.ProcedureName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Select(tmpSql.ToString()).Tables[0];
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void DropProcedures(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("DROP PROCEDURE  {0}", arguments.ProcedureName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void CreateGETDATA(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE OR REPLACE PROCEDURE GET_DATA_{0}
                                    (PS_INTERVAL_TYPE IN VARCHAR2,
                                     PI_INTERVAL_PERIOD IN NUMBER,
                                     PD_PROC_END_TIME IN DATE
                                    )
                                    IS", arguments.DBLinkName);




                tmpSql.AppendFormat(@"  TYPE SEG_STAT_TYPE IS REF CURSOR;
                                        SEG_STAT_CUR          SEG_STAT_TYPE;
                                        SEG_STAT_REC          RAW_DBA_HIST_SEG_STAT_{0}%ROWTYPE;
                                    ", arguments.DBLinkName);

                tmpSql.AppendFormat(@"  TYPE SQLSTAT_TYPE IS REF CURSOR;
                                        SQLSTAT_CUR          SQLSTAT_TYPE;
                                        SQLSTAT_REC          RAW_DBA_HIST_SQLSTAT_{0}%ROWTYPE;
                                    ", arguments.DBLinkName);

                tmpSql.AppendFormat(@"    LOG_REC     LOG_TAB%ROWTYPE;
                                    MIN_SNAP_ID RAW_DBA_HIST_SNAPSHOT_{0}.SNAP_ID%TYPE;
                                    MAX_SNAP_ID RAW_DBA_HIST_SNAPSHOT_{0}.SNAP_ID%TYPE;
                                    V_START_TIME DATE;
                                    V_END_TIME   DATE;
                                    V_SQL VARCHAR2(1000);
                                    ", arguments.DBLinkName);

                tmpSql.AppendFormat(@"BEGIN
                                        DBMS_OUTPUT.Enable(1000000);
                                        INIT_LOG_REC(LOG_REC);
                                        LOG_REC.PROCESS_NAME := 'GET_DATA_{0}';
                                           LOG_REC.START_TIME   := TO_CHAR(SYSTIMESTAMP, 'YYYYMMDDHH24MISSFF6');
                                           LOG_REC.END_TIME     := NULL;
                                           LOG_REC.SUCCESS_FLAG := ' ';
                                           LOG_REC.ERROR_CODE   :=  0 ;
                                           LOG_REC.ERROR_MSG    := ' ';
                                           LOG_WRITE(LOG_REC);

                                      COMMIT;
                                    ", arguments.DBLinkName);

                tmpSql.Append(@"    V_SQL := 'SELECT ';
   
                                   IF PS_INTERVAL_TYPE = 'D' THEN
                                      V_SQL := V_SQL || ':END_TIME - INTERVAL ''' || PI_INTERVAL_PERIOD || ''' DAY AS from_time, :END_TIME AS to_time';
                                   ELSIF PS_INTERVAL_TYPE = 'H' THEN
                                      V_SQL := V_SQL || ':END_TIME - INTERVAL ''' || PI_INTERVAL_PERIOD || ''' HOUR AS from_time, :END_TIME AS to_time';
                                   ELSE
                                      RAISE_APPLICATION_ERROR(-20011, 'Input Parameter(INTERVAL_TYPE) Exception');
                                   END IF;
                                   V_SQL := V_SQL || ' FROM DUAL';      
   
                                   EXECUTE IMMEDIATE V_SQL INTO V_START_TIME, V_END_TIME USING PD_PROC_END_TIME, PD_PROC_END_TIME;
   
                                   DBMS_OUTPUT.PUT_LINE(to_char(V_START_TIME, 'YYYYMMDDHH24MISS') || '##' || to_char(V_END_TIME, 'YYYYMMDDHH24MISS'));
                                    ");

                tmpSql.AppendFormat(@"BEGIN
                                        DBMS_OUTPUT.Enable(1000000);
                                        INIT_LOG_REC(LOG_REC);
                                        LOG_REC.PROCESS_NAME := 'GET_DATA_{0}';
                                           LOG_REC.START_TIME   := TO_CHAR(SYSTIMESTAMP, 'YYYYMMDDHH24MISSFF6');
                                           LOG_REC.END_TIME     := NULL;
                                           LOG_REC.SUCCESS_FLAG := ' ';
                                           LOG_REC.ERROR_CODE   :=  0 ;
                                           LOG_REC.ERROR_MSG    := ' ';
                                           LOG_WRITE(LOG_REC);

                                      COMMIT;
                                    ", arguments.DBLinkName);

                //1. DBA_HIST_SNAPSHOT
                tmpSql.AppendFormat(@" LOG_REC.TABLE_NAME := 'DBA_HIST_SNAPSHOT';
                                       DELETE RAW_DBA_HIST_SNAPSHOT_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                       INSERT INTO RAW_DBA_HIST_SNAPSHOT_{0} VALUE (SELECT * FROM DBA_HIST_SNAPSHOT@{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID);
   
                                       DBMS_OUTPUT.PUT_LINE('Table 1 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));
                                    ", arguments.DBLinkName);

                //2. DBA_HIST_SYSMETRIC_SUMMARY
                tmpSql.AppendFormat(@"  LOG_REC.TABLE_NAME := 'DBA_HIST_SYSMETRIC_SUMMARY';
                                   DELETE RAW_DBA_HIST_SYSMETRIC_SUMMARY_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_SYSMETRIC_SUMMARY_{0} VALUE (SELECT * FROM DBA_HIST_SYSMETRIC_SUMMARY@{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 2 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));
                                    ", arguments.DBLinkName);

                //3. DBA_HIST_SYSSTAT
                tmpSql.AppendFormat(@"LOG_REC.TABLE_NAME := 'DBA_HIST_SYSSTAT';
                                   DELETE RAW_DBA_HIST_SYSSTAT_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_SYSSTAT_{0} VALUE (SELECT A.*, 
                                                                                        TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                        TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                   FROM DBA_HIST_SYSSTAT@{0} A,
                                                                                        DBA_HIST_SNAPSHOT@{0} B
                                                                                  WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
                                                                                    AND B.SNAP_ID = A.SNAP_ID
                                                                                    AND B.DBID = A.DBID
                                                                                    AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
                                                    
                                   DBMS_OUTPUT.PUT_LINE('Table 3 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));
                                    ", arguments.DBLinkName);

                //5. DBA_HIST_SQLSTAT
                tmpSql.AppendFormat(@"LOG_REC.TABLE_NAME := 'DBA_HIST_SQLSTAT';
                                   DELETE RAW_DBA_HIST_SQLSTAT_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_SQLSTAT_{0} VALUE (SELECT A.*, 
                                                                                        TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                        TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                   FROM DBA_HIST_SQLSTAT@{0} A,
		                                                                                    DBA_HIST_SNAPSHOT@{0} B
		                                                                              WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
		                                                                                AND B.SNAP_ID = A.SNAP_ID
		                                                                                AND B.DBID = A.DBID
		                                                                                AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);

                                    DBMS_OUTPUT.PUT_LINE('Table 5 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);

                //6. DBA_HIST_SQL_SUMMARY
                tmpSql.AppendFormat(@"LOG_REC.TABLE_NAME := 'DBA_HIST_SQL_SUMMARY';
                                   DELETE RAW_DBA_HIST_SQL_SUMMARY_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_SQL_SUMMARY_{0} VALUE (SELECT A.*, 
                                                                                            TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                            TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                     FROM DBA_HIST_SQL_SUMMARY@{0} A,
			                                                                                    DBA_HIST_SNAPSHOT@{0} B
			                                                                              WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
			                                                                                AND B.SNAP_ID = A.SNAP_ID
			                                                                                AND B.DBID = A.DBID
			                                                                                AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
			                                                
                                   DBMS_OUTPUT.PUT_LINE('Table 6 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);

                //7. DBA_HIST_SQL_WORKAREA_HSTGRM
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_SQL_WORKAREA_HSTGRM';
                                   DELETE RAW_DBA_HIST_SQL_WORKAREA_HSTGRM_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_SQL_WORKAREA_HSTGRM_{0} VALUE (SELECT A.*, 
                                                                                                    TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                                    TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                            FROM DBA_HIST_SQL_WORKAREA_HSTGRM@{0} A,
							                                                                                   DBA_HIST_SNAPSHOT@{0} B
							                                                                             WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
							                                                                               AND B.SNAP_ID = A.SNAP_ID
							                                                                               AND B.DBID = A.DBID
							                                                                               AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 7 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));   

                                    ", arguments.DBLinkName);

                //8. DBA_HIST_LIBRARYCACHE
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_LIBRARYCACHE';
                                   DELETE RAW_DBA_HIST_LIBRARYCACHE_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_LIBRARYCACHE_{0} VALUE (SELECT A.*, 
                                                                                             TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                             TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                        FROM DBA_HIST_LIBRARYCACHE@{0} A,
					                                                                                   DBA_HIST_SNAPSHOT@{0} B
					                                                                             WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
					                                                                               AND B.SNAP_ID = A.SNAP_ID
					                                                                               AND B.DBID = A.DBID
					                                                                               AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 8 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6')); 

                                    ", arguments.DBLinkName);

                //9. DBA_HIST_LATCH
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_LATCH';
                                   DELETE RAW_DBA_HIST_LATCH_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_LATCH_{0} VALUE (SELECT A.*, 
                                                                                      TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                      TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                 FROM DBA_HIST_LATCH@{0} A,
	                                                                                    DBA_HIST_SNAPSHOT@{0} B
	                                                                              WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
	                                                                                AND B.SNAP_ID = A.SNAP_ID
	                                                                                AND B.DBID = A.DBID
	                                                                                AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 9 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);


                //10. DBA_HIST_WAITSTAT
                tmpSql.AppendFormat(@"  LOG_REC.TABLE_NAME := 'DBA_HIST_WAITSTAT';
                                   DELETE RAW_DBA_HIST_WAITSTAT_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_WAITSTAT_{0} VALUE (SELECT A.*, 
                                                                                         TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                         TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                    FROM DBA_HIST_WAITSTAT@{0} A,
		                                                                                     DBA_HIST_SNAPSHOT@{0} B
		                                                                               WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
		                                                                                 AND B.SNAP_ID = A.SNAP_ID
		                                                                                 AND B.DBID = A.DBID
		                                                                                 AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 10 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);


                //11. DBA_HIST_DLM_MISC
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_DLM_MISC';
                                   DELETE RAW_DBA_HIST_DLM_MISC_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_DLM_MISC_{0} VALUE (SELECT A.*, 
                                                                                         TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                         TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                    FROM DBA_HIST_DLM_MISC@{0} A,
		                                                                                     DBA_HIST_SNAPSHOT@{0} B
		                                                                               WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
		                                                                                 AND B.SNAP_ID = A.SNAP_ID
		                                                                                 AND B.DBID = A.DBID
		                                                                                 AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 11 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);


                //12. DBA_HIST_CURRENT_BLOCK_SERVER
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_CURRENT_BLOCK_SERVER';
                                   DELETE RAW_DBA_HIST_CURRENT_BLOCK_SERVER_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_CURRENT_BLOCK_SERVER_{0} VALUE (SELECT A.*, 
                                                                                                     TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                                     TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                                FROM DBA_HIST_CURRENT_BLOCK_SERVER@{0} A,
								                                                                                     DBA_HIST_SNAPSHOT@{0} B
								                                                                               WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
								                                                                                 AND B.SNAP_ID = A.SNAP_ID
								                                                                                 AND B.DBID = A.DBID
								                                                                                 AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 12 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);


                //13. DBA_HIST_CR_BLOCK_SERVER
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_CR_BLOCK_SERVER';
                                   DELETE RAW_DBA_HIST_CR_BLOCK_SERVER_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_CR_BLOCK_SERVER_{0} VALUE (SELECT A.*, 
                                                                                                TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                                TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                           FROM DBA_HIST_CR_BLOCK_SERVER@{0} A,
						                                                                                    DBA_HIST_SNAPSHOT@{0} B
						                                                                              WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
						                                                                                AND B.SNAP_ID = A.SNAP_ID
						                                                                                AND B.DBID = A.DBID
						                                                                                AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 13 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);

                //14. DBA_HIST_SYSTEM_EVENT
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_SYSTEM_EVENT';
                                   DELETE RAW_DBA_HIST_SYSTEM_EVENT_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_SYSTEM_EVENT_{0} VALUE (SELECT A.*, 
                                                                                             TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                             TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                        FROM DBA_HIST_SYSTEM_EVENT@{0} A,
					                                                                                   DBA_HIST_SNAPSHOT@{0} B
					                                                                             WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
					                                                                               AND B.SNAP_ID = A.SNAP_ID
					                                                                               AND B.DBID = A.DBID
					                                                                               AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 14 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);


                //15. DBA_HIST_SYS_TIME_MODEL
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_SYS_TIME_MODEL';
                                   DELETE RAW_DBA_HIST_SYS_TIME_MODEL_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_SYS_TIME_MODEL_{0} VALUE (SELECT A.*, 
                                                                                               TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                               TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                          FROM DBA_HIST_SYS_TIME_MODEL@{0} A,
					                                                                                     DBA_HIST_SNAPSHOT@{0} B
					                                                                               WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
					                                                                                 AND B.SNAP_ID = A.SNAP_ID
					                                                                                 AND B.DBID = A.DBID
					                                                                                 AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 15 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);


                //16. DBA_HIST_OSSTAT
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_OSSTAT';
                                   DELETE RAW_DBA_HIST_OSSTAT_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_OSSTAT_{0} VALUE (SELECT A.*, 
                                                                                       TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                       TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                  FROM DBA_HIST_OSSTAT@{0} A,
	                                                                                     DBA_HIST_SNAPSHOT@{0} B
	                                                                               WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
	                                                                                 AND B.SNAP_ID = A.SNAP_ID
	                                                                                 AND B.DBID = A.DBID
	                                                                                 AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 16 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);


                //17. DBA_HIST_PGASTAT
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_PGASTAT';
                                   DELETE RAW_DBA_HIST_PGASTAT_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_PGASTAT_{0} VALUE (SELECT A.*, 
                                                                                        TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                        TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
	                                                                                 FROM DBA_HIST_PGASTAT@{0} A,
		                                                                                    DBA_HIST_SNAPSHOT@{0} B
		                                                                              WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
		                                                                                AND B.SNAP_ID = A.SNAP_ID
		                                                                                AND B.DBID = A.DBID
		                                                                                AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 17 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);

                //18. DBA_HIST_RESOURCE_LIMIT
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_RESOURCE_LIMIT';
                                   DELETE RAW_DBA_HIST_RESOURCE_LIMIT_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_RESOURCE_LIMIT_{0} VALUE (SELECT A.*, 
			                                                                                         TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
			                                                                                         TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                          FROM DBA_HIST_RESOURCE_LIMIT@{0} A,
					                                                                                     DBA_HIST_SNAPSHOT@{0} B
					                                                                               WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
					                                                                                 AND B.SNAP_ID = A.SNAP_ID
					                                                                                 AND B.DBID = A.DBID
					                                                                                 AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 18 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);

                //19. DBA_HIST_BUFFER_POOL_STAT
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_BUFFER_POOL_STAT';
                                   DELETE RAW_DBA_HIST_BUFFER_POOL_STAT_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_BUFFER_POOL_STAT_{0} VALUE (SELECT A.*, 
			                                                                                           TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
			                                                                                           TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS') 
			                                                                                      FROM DBA_HIST_BUFFER_POOL_STAT@{0} A,
					                                                                                       DBA_HIST_SNAPSHOT@{0} B
					                                                                                 WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
					                                                                                   AND B.SNAP_ID = A.SNAP_ID
					                                                                                   AND B.DBID = A.DBID
					                                                                                   AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 19 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);

                //20. DBA_HIST_ENQUEUE_STAT
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_ENQUEUE_STAT';
                                   DELETE RAW_DBA_HIST_ENQUEUE_STAT_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_ENQUEUE_STAT_{0} VALUE (SELECT A.*, 
	                                                                                           TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
	                                                                                           TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
	                                                                                      FROM DBA_HIST_ENQUEUE_STAT@{0} A,
			                                                                                       DBA_HIST_SNAPSHOT@{0} B
			                                                                                 WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
			                                                                                   AND B.SNAP_ID = A.SNAP_ID
			                                                                                   AND B.DBID = A.DBID
			                                                                                   AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 20 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);


                //21. DBA_HIST_ROWCACHE_SUMMARY
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_ROWCACHE_SUMMARY';
                                   DELETE RAW_DBA_HIST_ROWCACHE_SUMMARY_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_ROWCACHE_SUMMARY_{0} VALUE (SELECT A.*, 
			                                                                                           TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
			                                                                                           TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
			                                                                                      FROM DBA_HIST_ROWCACHE_SUMMARY@{0} A,
					                                                                                       DBA_HIST_SNAPSHOT@{0} B
					                                                                                 WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
					                                                                                   AND B.SNAP_ID = A.SNAP_ID
					                                                                                   AND B.DBID = A.DBID
					                                                                                   AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 21 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);


                //22. DBA_HIST_SGASTAT
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_ROWCACHE_SUMMARY';
                                   DELETE RAW_DBA_HIST_ROWCACHE_SUMMARY_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_ROWCACHE_SUMMARY_{0} VALUE (SELECT A.*, 
			                                                                                           TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
			                                                                                           TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
			                                                                                      FROM DBA_HIST_ROWCACHE_SUMMARY@{0} A,
					                                                                                       DBA_HIST_SNAPSHOT@{0} B
					                                                                                 WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
					                                                                                   AND B.SNAP_ID = A.SNAP_ID
					                                                                                   AND B.DBID = A.DBID
					                                                                                   AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 21 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);


                //23. DBA_HIST_PARAMETER
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_PARAMETER';
                                   DELETE RAW_DBA_HIST_PARAMETER_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_PARAMETER_{0} VALUE (SELECT A.*, 
                                                                                          TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                          TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                     FROM DBA_HIST_PARAMETER@{0} A,
                                                                                          DBA_HIST_SNAPSHOT@{0} B
                                                                                    WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
                                                                                      AND B.SNAP_ID = A.SNAP_ID
                                                                                      AND B.DBID = A.DBID
                                                                                      AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 23 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);


                //24. DBA_HIST_SGA
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_SGA';
                                   DELETE RAW_DBA_HIST_SGA_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_SGA_{0} VALUE (SELECT A.*, 
                                                                                    TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                    TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                               FROM DBA_HIST_SGA@{0} A,
                                                                                    DBA_HIST_SNAPSHOT@{0} B
                                                                              WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
                                                                                AND B.SNAP_ID = A.SNAP_ID
                                                                                AND B.DBID = A.DBID
                                                                                AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);

                                   DBMS_OUTPUT.PUT_LINE('Table 24 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));   

                                    ", arguments.DBLinkName);

                //25. DBA_HIST_ACTIVE_SESS_HISTORY
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_ACTIVE_SESS_HISTORY';
                                   DELETE RAW_DBA_HIST_ACTIVE_SESS_HISTORY_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_ACTIVE_SESS_HISTORY_{0} VALUE (SELECT A.*, 
                                                                                                    TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                                    TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                               FROM DBA_HIST_ACTIVE_SESS_HISTORY@{0} A,
										                                                                                DBA_HIST_SNAPSHOT@{0} B
										                                                                          WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
										                                                                            AND B.SNAP_ID = A.SNAP_ID
										                                                                            AND B.DBID = A.DBID
										                                                                            AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 25 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));    

                                    ", arguments.DBLinkName);



                //26. DBA_HIST_LATCH_MISSES_SUMMARY
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_LATCH_MISSES_SUMMARY';
                                   DELETE RAW_DBA_HIST_LATCH_MISSES_SUMMARY_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_LATCH_MISSES_SUMMARY_{0} VALUE (SELECT A.*, 
                                                                                                     TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                                     TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                                FROM DBA_HIST_LATCH_MISSES_SUMMARY@{0} A,
										                                                                                 DBA_HIST_SNAPSHOT@{0} B
										                                                                           WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
										                                                                             AND B.SNAP_ID = A.SNAP_ID
										                                                                             AND B.DBID = A.DBID
										                                                                             AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
										                                             
                                   DBMS_OUTPUT.PUT_LINE('Table 26 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));    

                                    ", arguments.DBLinkName);


                //27. DBA_HIST_THREAD
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_THREAD';
                                   DELETE RAW_DBA_HIST_THREAD_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_THREAD_{0} VALUE (SELECT A.*, 
                                                                                       TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                       TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                  FROM DBA_HIST_THREAD@{0} A,
			                                                                                 DBA_HIST_SNAPSHOT@{0} B
			                                                                           WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
			                                                                             AND B.SNAP_ID = A.SNAP_ID
			                                                                             AND B.DBID = A.DBID
			                                                                             AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 27 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6')); 

                                    ", arguments.DBLinkName);


                //28. DBA_HIST_SEG_STAT
                tmpSql.AppendFormat(@"   LOG_REC.TABLE_NAME := 'DBA_HIST_SEG_STAT';
                                   DELETE RAW_DBA_HIST_SEG_STAT_{0} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;
   
                                   INSERT INTO RAW_DBA_HIST_SEG_STAT_{0} VALUE (SELECT A.*, 
                                                                                         TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS'),
                                                                                         TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, 'YYYY-MM-DD HH24:MI:SS'), 'YYYY-MM-DD HH24:MI:SS')
                                                                                    FROM DBA_HIST_SEG_STAT@{0} A,
			                                                                                   DBA_HIST_SNAPSHOT@{0} B
			                                                                             WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID
			                                                                               AND B.SNAP_ID = A.SNAP_ID
			                                                                               AND B.DBID = A.DBID
			                                                                               AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);
   
                                   DBMS_OUTPUT.PUT_LINE('Table 28 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);


                //
                tmpSql.AppendFormat(@"   OPEN SEG_STAT_CUR FOR
                                          SELECT DISTINCT DBID, TS#, OBJ#, DATAOBJ#, CON_DBID
                                            FROM RAW_DBA_HIST_SEG_STAT_{0}
                                           WHERE SNAP_ID >= MIN_SNAP_ID
                                             AND SNAP_ID <= MAX_SNAP_ID;

                                        LOOP
                                                FETCH SEG_STAT_CUR INTO SEG_STAT_REC.DBID, SEG_STAT_REC.TS#, SEG_STAT_REC.OBJ#, SEG_STAT_REC.DATAOBJ#, SEG_STAT_REC.CON_DBID;
                                                EXIT WHEN SEG_STAT_CUR%NOTFOUND;
                                    ", arguments.DBLinkName);


                //29. DBA_HIST_SEG_STAT_OBJ
                tmpSql.AppendFormat(@"LOG_REC.TABLE_NAME := 'DBA_HIST_SEG_STAT_OBJ';
                                      MERGE INTO ISIA.RAW_DBA_HIST_SEG_STAT_OBJ_{0} d
		                                        USING (SELECT *
		                                                 FROM DBA_HIST_SEG_STAT_OBJ@{0}
		                                                WHERE DBID = SEG_STAT_REC.DBID
		                                                  AND TS# = SEG_STAT_REC.TS#
		                                                  AND OBJ# = SEG_STAT_REC.OBJ#
		                                                  AND DATAOBJ# = SEG_STAT_REC.DATAOBJ#
		                                                  AND CON_DBID = SEG_STAT_REC.CON_DBID) s
		                                           ON (d.DBID = s.DBID
		                                               AND d.TS# = s.TS#
		                                               AND d.OBJ# = s.OBJ#
		                                               AND d.DATAOBJ# = s.DATAOBJ#
		                                               AND d.CON_DBID = s.CON_DBID)
					                                   WHEN NOT MATCHED
					                                   THEN
					                                       INSERT   (DBID,
													                                 TS#,
													                                 OBJ#,
													                                 DATAOBJ#,
													                                 OWNER,
													                                 OBJECT_NAME,
													                                 SUBOBJECT_NAME,
													                                 OBJECT_TYPE,
													                                 TABLESPACE_NAME,
													                                 PARTITION_TYPE,
													                                 CON_DBID,
													                                 CON_ID,
													                                 INSERT_TIME)
				                                           VALUES (S.DBID,
													                                 S.TS#,
													                                 S.OBJ#,
													                                 S.DATAOBJ#,
													                                 S.OWNER,
													                                 S.OBJECT_NAME,
													                                 S.SUBOBJECT_NAME,
													                                 S.OBJECT_TYPE,
													                                 S.TABLESPACE_NAME,
													                                 S.PARTITION_TYPE,
													                                 S.CON_DBID,
													                                 S.CON_ID,
													                                 SYSDATE);
      
                                   END LOOP;
                                   CLOSE SEG_STAT_CUR;
   
                                   DBMS_OUTPUT.PUT_LINE('Table 29 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));
   
                                   OPEN SQLSTAT_CUR FOR
                                      SELECT DISTINCT DBID, SQL_ID, CON_DBID
                                        FROM DBA_HIST_SQLSTAT@{0}
                                       WHERE SNAP_ID >= MIN_SNAP_ID
                                         AND SNAP_ID <= MAX_SNAP_ID;

                                   LOOP
                                      FETCH SQLSTAT_CUR INTO SQLSTAT_REC.DBID, SQLSTAT_REC.SQL_ID, SQLSTAT_REC.CON_DBID;
                                      EXIT WHEN SQLSTAT_CUR%NOTFOUND;

                                    ", arguments.DBLinkName);



                //31. DBA_HIST_SQL_BIND_METADATA
                tmpSql.AppendFormat(@"LOG_REC.TABLE_NAME := 'DBA_HIST_SQL_BIND_METADATA';
	                                MERGE INTO ISIA.RAW_DBA_HIST_SQL_BIND_METADATA_{0} d
	                                USING (
	                                  Select
	                                    DBID,
	                                    SQL_ID,
	                                    NAME,
	                                    POSITION,
	                                    DUP_POSITION,
	                                    DATATYPE,
	                                    DATATYPE_STRING,
	                                    CHARACTER_SID,
	                                    PRECISION,
	                                    SCALE,
	                                    MAX_LENGTH,
	                                    CON_DBID,
	                                    CON_ID
	                                  FROM DBA_HIST_SQL_BIND_METADATA@{0}
	                                  WHERE DBID = SQLSTAT_REC.DBID
	                                    AND SQL_ID = SQLSTAT_REC.SQL_ID
	                                    AND CON_DBID = SQLSTAT_REC.CON_DBID) s
	                                ON
	                                  (d.DBID = s.DBID and
	                                  d.SQL_ID = s.SQL_ID and
	                                  d.POSITION = s.POSITION and
	                                  d.CON_DBID = s.CON_DBID)
	                                WHEN NOT MATCHED
	                                THEN
	                                INSERT (
	                                  DBID, SQL_ID, NAME,aaaaaaaaaaaaaaaaa
	                                  POSITION, DUP_POSITION, DATATYPE,
	                                  DATATYPE_STRING, CHARACTER_SID, PRECISION,
	                                  SCALE, MAX_LENGTH, CON_DBID,
	                                  CON_ID,
	                                  INSERT_TIME)
	                                VALUES (
	                                  s.DBID, s.SQL_ID, s.NAME,
	                                  s.POSITION, s.DUP_POSITION, s.DATATYPE,
	                                  s.DATATYPE_STRING, s.CHARACTER_SID, s.PRECISION,
	                                  s.SCALE, s.MAX_LENGTH, s.CON_DBID,
	                                  s.CON_ID,
	                                  SYSDATE);
	    

                                    ", arguments.DBLinkName);


                //32. DBA_HIST_SQL_PLAN
                tmpSql.AppendFormat(@" LOG_REC.TABLE_NAME := 'DBA_HIST_SQL_PLAN'; 
	                                MERGE INTO ISIA.RAW_DBA_HIST_SQL_PLAN_{0} d
	                                 USING (SELECT DBID,
	                                               SQL_ID,
	                                               PLAN_HASH_VALUE,
	                                               ID,
	                                               OPERATION,
	                                               OPTIONS,
	                                               OBJECT_NODE,
	                                               OBJECT#,
	                                               OBJECT_OWNER,
	                                               OBJECT_NAME,
	                                               OBJECT_ALIAS,
	                                               OBJECT_TYPE,
	                                               OPTIMIZER,
	                                               PARENT_ID,
	                                               DEPTH,
	                                               POSITION,
	                                               SEARCH_COLUMNS,
	                                               COST,
	                                               CARDINALITY,
	                                               BYTES,
	                                               OTHER_TAG,
	                                               PARTITION_START,
	                                               PARTITION_STOP,
	                                               PARTITION_ID,
	                                               OTHER,
	                                               DISTRIBUTION,
	                                               CPU_COST,
	                                               IO_COST,
	                                               TEMP_SPACE,
	                                               ACCESS_PREDICATES,
	                                               FILTER_PREDICATES,
	                                               PROJECTION,
	                                               TIME,
	                                               QBLOCK_NAME,
	                                               REMARKS,
	                                               TIMESTAMP,
	                                               OTHER_XML,
	                                               CON_DBID,
	                                               CON_ID
	                                          FROM DBA_HIST_SQL_PLAN@{0}
	                                          WHERE DBID = SQLSTAT_REC.DBID
	                                            AND SQL_ID = SQLSTAT_REC.SQL_ID
	                                            AND CON_DBID = SQLSTAT_REC.CON_DBID) s
	                                    ON (    d.DBID = s.DBID
	                                        AND d.SQL_ID = s.SQL_ID
	                                        AND d.PLAN_HASH_VALUE = s.PLAN_HASH_VALUE
	                                        AND d.ID = s.ID
	                                        AND d.CON_DBID = s.CON_DBID)
			                             WHEN NOT MATCHED
			                             THEN
			                                 INSERT     (DBID,
			                                             SQL_ID,
			                                             PLAN_HASH_VALUE,
			                                             ID,
			                                             OPERATION,
			                                             OPTIONS,
			                                             OBJECT_NODE,
			                                             OBJECT#,
			                                             OBJECT_OWNER,
			                                             OBJECT_NAME,
			                                             OBJECT_ALIAS,
			                                             OBJECT_TYPE,
			                                             OPTIMIZER,
			                                             PARENT_ID,
			                                             DEPTH,
			                                             POSITION,
			                                             SEARCH_COLUMNS,
			                                             COST,
			                                             CARDINALITY,
			                                             BYTES,
			                                             OTHER_TAG,
			                                             PARTITION_START,
			                                             PARTITION_STOP,
			                                             PARTITION_ID,
			                                             OTHER,
			                                             DISTRIBUTION,
			                                             CPU_COST,
			                                             IO_COST,
			                                             TEMP_SPACE,
			                                             ACCESS_PREDICATES,
			                                             FILTER_PREDICATES,
			                                             PROJECTION,
			                                             TIME,
			                                             QBLOCK_NAME,
			                                             REMARKS,
			                                             TIMESTAMP,
			                                             OTHER_XML,
			                                             CON_DBID,
			                                             CON_ID,
			                                             INSERT_TIME)
			                                     VALUES (s.DBID,
			                                             s.SQL_ID,
			                                             s.PLAN_HASH_VALUE,
			                                             s.ID,
			                                             s.OPERATION,
			                                             s.OPTIONS,
			                                             s.OBJECT_NODE,
			                                             s.OBJECT#,
			                                             s.OBJECT_OWNER,
			                                             s.OBJECT_NAME,
			                                             s.OBJECT_ALIAS,
			                                             s.OBJECT_TYPE,
			                                             s.OPTIMIZER,
			                                             s.PARENT_ID,
			                                             s.DEPTH,
			                                             s.POSITION,
			                                             s.SEARCH_COLUMNS,
			                                             s.COST,
			                                             s.CARDINALITY,
			                                             s.BYTES,
			                                             s.OTHER_TAG,
			                                             s.PARTITION_START,
			                                             s.PARTITION_STOP,
			                                             s.PARTITION_ID,
			                                             s.OTHER,
			                                             s.DISTRIBUTION,
			                                             s.CPU_COST,
			                                             s.IO_COST,
			                                             s.TEMP_SPACE,
			                                             s.ACCESS_PREDICATES,
			                                             s.FILTER_PREDICATES,
			                                             s.PROJECTION,
			                                             s.TIME,
			                                             s.QBLOCK_NAME,
			                                             s.REMARKS,
			                                             s.TIMESTAMP,
			                                             s.OTHER_XML,
			                                             s.CON_DBID,
			                                             s.CON_ID,
			                                             SYSDATE);
      
                               END LOOP;
                               CLOSE SQLSTAT_CUR;
   
                               DBMS_OUTPUT.PUT_LINE('Table 30~32 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));

                                    ", arguments.DBLinkName);


                tmpSql.Append(@"LOG_REC.END_TIME     := TO_CHAR(SYSTIMESTAMP, 'YYYYMMDDHH24MISSFF6');
                            LOG_REC.SUCCESS_FLAG := 'Y';
                            LOG_WRITE(LOG_REC);
   
                            COMMIT;
   
                        EXCEPTION
                            WHEN OTHERS
                            THEN
                                ROLLBACK;

                                LOG_REC.END_TIME     := TO_CHAR(SYSTIMESTAMP, 'YYYYMMDDHH24MISSFF6');
                                LOG_REC.SUCCESS_FLAG := 'N';
                                LOG_REC.ERROR_CODE   := SQLCODE ;
                                LOG_REC.ERROR_MSG    := SUBSTRB(SQLERRM,1,4000);
                                LOG_WRITE(LOG_REC);

                                COMMIT;
                        END GET_DATA_{0}; 
                            ");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


  
    }
}
