package com.task.bean;

import java.util.ArrayList;
import java.util.Hashtable;
import java.util.Iterator;
import java.util.List;

import org.apache.log4j.Logger;
import org.quartz.Job;
import org.quartz.JobExecutionContext;
import org.quartz.JobExecutionException;

import com.task.check.CheckPoint;
import com.task.common.DataPool;
import com.task.common.DataTypeUtil;
import com.task.common.DateUtil;
import com.task.common.PropertysUtil;
import com.task.db.DBOperator;

public class FileExchange implements Job{
	
	private static Logger logger = Logger.getLogger( FileExchange.class );
	
	private DBOperator dbo = new DBOperator();
	
	private DataTypeUtil typeUtil = new DataTypeUtil();
	
	private static Logger log = Logger.getLogger(FileExchange.class);
	/**
	 * 从 资料交换实例档 获得信息存入 界面交换实例档
	 * @param connectInfo 数据库连接信息
	 * @throws Exception
	 */
	public void getData(Hashtable connectInfo) throws Exception{
		try{
			log.info("start getData from ："+connectInfo.get("Url"));	
			String typeStr = typeUtil.getINTFDataType();
			
			String notCODE = "'B2B_PO','B2B_POG','B2B_RC','B2B_RTV','INTC_SKU','REGEVT'";//不通过interface抛转的CHGCODE
			String sqlIns = "select * from "+connectInfo.get("InstLib")+".CHGINST where ((CHGTYP='O' and  CHGSTS<>'2' and  CHGSTS<>'0') or (CHGTYP='O' and  CHGSTS is null)) and CHGCOD not in ("+notCODE+","+typeStr+")";//,'INTC_STORE'
			//log.info("sqlIns="+sqlIns);
			//CRM&SOM相关CHGCODE : CRM_MEM,CRM_SCORE,CRM_O_CDE,CRM_O_DPT,CRM_O_PCD,CRM_O_SKU,CRM_O_STR,CRM_ISALES,SOM_SALE  
			//SQL FOR CRM&SOM
//			String crm_som_code = "'CRM_MEM','CRM_SCORE','CRM_O_CDE','CRM_O_DPT','CRM_O_PCD','CRM_O_SKU','CRM_O_STR','CRM_ISALES','SOM_SALE'";
//			String sqlIns = "select * from "+connectInfo.get("InstLib")+".CHGINST where ((CHGTYP='O' and  CHGSTS<>'2' and  CHGSTS<>'0') or (CHGTYP='O' and  CHGSTS is null)) and CHGCOD  in ("+crm_som_code+") ";
			
			Object[] objIns = new Object[0];
			List insList = dbo.getData(sqlIns, objIns, connectInfo);
			
			log.info("ins size："+insList.size()+" sqlIns="+sqlIns);
			
			String sqlInsFile = "select * from "+connectInfo.get("InstLib")+".CHGINSTF where INSTNO in (select INSTNO from "+connectInfo.get("InstLib")+".CHGINST where ((CHGTYP='O' and  CHGSTS<>'2' and  CHGSTS<>'0') or (CHGTYP='O' and  CHGSTS is null)) and CHGCOD  not in ("+notCODE+","+typeStr+"))";
			
//			SQL FOR CRM&SOM
//			String sqlInsFile = "select * from "+connectInfo.get("InstLib")+".CHGINSTF where INSTNO in (select INSTNO from "+connectInfo.get("InstLib")+".CHGINST where ((CHGTYP='O' and  CHGSTS<>'2' and  CHGSTS<>'0') or (CHGTYP='O' and  CHGSTS is null)) and CHGCOD  in ("+crm_som_code+"))";//,'INTC_STORE'
			//log.info("sqlInsFile="+sqlInsFile);
			
			List insFileList = dbo.getData(sqlInsFile, objIns, connectInfo);
			log.info("insfile size："+insList.size()+" sqlInsFile="+sqlInsFile);
			
			Hashtable insFileHT = new Hashtable();
			for(int i=0;i<insFileList.size();i++){
				Object[] obj = (Object[]) insFileList.get(i);
				String instNo = (String) obj[0];
				List list = null;
				if(insFileHT.containsKey(instNo.trim())){
					list = (List) insFileHT.get(instNo.trim());
				}else{
					list = new ArrayList();
				}
				list.add(obj);
				insFileHT.put(instNo.trim(), list);
			}
			
			String sqlInsUpdate = "update "+connectInfo.get("InstLib")+".CHGINST set CHGSTS=? where INSTNO=?";
			List updateArylist = new ArrayList();
			for(int i=0;i<insList.size();i++){
				Object[] insObj = (Object[]) insList.get(i);
				Object[] updateObj = new Object[2];
				updateObj[0] = "2";
				updateObj[1] = insObj[0].toString().trim();
				updateArylist.add(updateObj);
			}
			dbo.execDataByTrancation(connectInfo, sqlInsUpdate, updateArylist);
			
			this.putDataToCenter(insList, insFileHT, connectInfo);
			log.info("end getData from ："+connectInfo.get("Url"));
		}catch(Exception e){
			e.printStackTrace();
		}
	}
	
	public void putDataToCenter(List insList,Hashtable insFileHT,Hashtable connectInfo){
		String insNos = "";//记录错误的insNo
		//新增的标示，用于在报错时记录下 by mark
		String chgcode = "";
		//检查/更新点对象，用于更新状态会获取状态 by mark
		CheckPoint cp = CheckPoint.getInstance();
		try{
			String code = connectInfo.get("Code").toString();
			log.info("put data to datacenter from "+code+" start");

			
			Hashtable chgCntHT = new Hashtable();
			String sqlInsCenter = "insert into interface.HOLA_APP_CHG_INST (INSTNO,SYS_CODE,CHG_CODE,CHG_TYPE,SRCDATE,SRCCNT,SRCUSER,SRCSTATUS,TRGSERVER,FILECNT,CHGDATE,CHGCNT,CHGUSER,CHGSTATUS,SRCSERVER,OID) values (?,?,?,?,to_date(?,'yyyy-mm-dd hh24:mi:ss'),?,?,?,?,?,to_date(?,'yyyy-mm-dd hh24:mi:ss'),?,?,?,?,?)";
//			String sqlInsCenterTest = "insert into interfacetest.HOLA_APP_CHG_INST (INSTNO,SYS_CODE,CHG_CODE,CHG_TYPE,SRCDATE,SRCCNT,SRCUSER,SRCSTATUS,TRGSERVER,FILECNT,CHGDATE,CHGCNT,CHGUSER,CHGSTATUS,SRCSERVER,OID) values (?,?,?,?,to_date(?,'yyyy-mm-dd hh24:mi:ss'),?,?,?,?,?,to_date(?,'yyyy-mm-dd hh24:mi:ss'),?,?,?,?,?)";
			String srcSrv = "";
			for(int i=0;i<insList.size();i++){
				//如果查询到这批资料中有错误的内容，就跳过，不再做处理 by mark
				//这个动作每次都会查询一次数据库，考虑到可能有多个程序同时运行(每个都在独立的内存环境内)，因此不能通过全局变量控制
				if(cp.getState()==false)
					continue;
				
				List objAryList = new ArrayList();
				
				Object[] obj = (Object[]) insList.get(i);
				Object[] objInsCenter = new Object[16];
				
				//每次都保存一个新的值，如果发生异常就可以通过chgcode来更新状态。 by mark
				if(obj[1]!=null)
					chgcode = obj[1].toString();
				cp.setCode(chgcode);
				
				String oid = java.util.UUID.randomUUID().toString().split("-")[0];//Uuid16.create().toString();
					      oid = code + oid ;
				log.info("instno duplicate = "+oid+"    oinstno="+obj[0]);
				objInsCenter[0] = oid;
				if(obj[16]!=null){
					objInsCenter[1] = obj[16].toString().trim();//SYS_CODE
				}else{
					objInsCenter[1] = "";//SYS_CODE
				}
				
				
				if(obj[1]!=null){
					objInsCenter[2] = obj[1].toString().trim();//CHG_CODE
				}else{
					objInsCenter[2] = "";
				}
				if(obj[2]!=null){
					objInsCenter[3] = obj[2].toString().trim();//CHG_TYPE
				}else{
					objInsCenter[3] = "";
				}
//				System.out.println("obj[3]="+obj[3]+"      obj[4]="+obj[4]);
				if(obj[3]!=null){
					if(obj[3].toString().trim().length()==8){
						String datestr = obj[3].toString().trim().substring(0,4)+"-"+obj[3].toString().trim().substring(4,6)+"-"+obj[3].toString().trim().substring(6,8);
						if(obj[4].toString().trim().length()==5){
							obj[4] = "0"+obj[4].toString().trim();//JDA日期不会左补0
						}
						if(obj[4].toString().trim().length()==3){
							obj[4] = "000"+obj[4].toString().trim();//JDA日期不会左补0
						}
						if(obj[4].toString().trim().length()==2){
							obj[4] = "0000"+obj[4].toString().trim();//JDA日期不会左补0
						}
						if(obj[4].toString().trim().length()==1){
							obj[4] = "00000"+obj[4].toString().trim();//JDA日期不会左补0
						}
						String timestr = obj[4].toString().trim().substring(0,2)+":"+obj[4].toString().trim().substring(2,4)+":"+obj[4].toString().trim().substring(4,6);
//						System.out.println("datestr="+datestr+"      timestr="+timestr);
						objInsCenter[4] = datestr+" "+timestr;
					}else{
						objInsCenter[4] = obj[3].toString().trim();//SRCDATE 
					}
					
				}else{
					objInsCenter[4] = "";//SRCDATE 
				}
				if(obj[5]!=null){
					objInsCenter[5] = obj[5].toString().trim();//SRCCNT
				}else{
					objInsCenter[5] = "";
				}
				if(obj[6]!=null){
					objInsCenter[6] = obj[6].toString().trim();//SRCUSER
				}else{
					objInsCenter[6] = "";
				}
				if(obj[7]!=null){
					objInsCenter[7] = obj[7].toString().trim();//SRCSTATUS
				}else{
					objInsCenter[7] = "";
				}
				if(obj[8]!=null){
					objInsCenter[8] = obj[8].toString().trim();//TRGSERVER
				}else{
					objInsCenter[8] = "";
				}
				if(obj[9]!=null){
					objInsCenter[9] = obj[9].toString().trim();//FILECNT
				}else{
					objInsCenter[9] = "";
				}
				objInsCenter[10] = DateUtil.formatDate3(DateUtil.getCurrentDate());//CHGDATE obj[10]
				if(obj[11]!=null){
					objInsCenter[11] = obj[11].toString().trim();//CHGCNT
				}else{
					objInsCenter[11] = "";
				}
//				if(obj[12]!=null){
//					objInsCenter[12] = obj[12].toString().trim();//CHGUSER
//				}else{
					objInsCenter[12] = "interface";
//				}
//				if(obj[13]!=null){
//					objInsCenter[13] = obj[13].toString().trim();//CHGSTATUS
//				}else{
					objInsCenter[13] = "1";
//				}
				objInsCenter[14] = connectInfo.get("Code");
				if(connectInfo.get("Code").toString()!=null){
					srcSrv = connectInfo.get("Code").toString();
				}
				
				
				objInsCenter[15] = obj[0];//INSTNO
				
//				sqlList.add(sqlInsCenter);
				objAryList.add(objInsCenter);
				
//				sqlListTest.add(sqlInsCenterTest);
//				objAryListTest.add(objInsCenter);
				
				int chgcount = 0;
				String srclib = "";
				String sqlInsFileCenter = "insert into interface.HOLA_APP_CHG_INST_FILE (INSTNO,SRCNAME,SRCCNT,SRCSUM,SRCOTHER,CHGNAME,CHGCNT,CHGSUM,CHGOTHER,TARNAME,TARCNT,TARSUM,TAROTHER,SRCLIB,TEMPNAME,TARLIB,TEMPLIB) values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
//				String sqlInsFileCenterTest = "insert into interfacetest.HOLA_APP_CHG_INST_FILE (INSTNO,SRCNAME,SRCCNT,SRCSUM,SRCOTHER,CHGNAME,CHGCNT,CHGSUM,CHGOTHER,TARNAME,TARCNT,TARSUM,TAROTHER,SRCLIB,TEMPNAME,TARLIB,TEMPLIB) values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
				List insFileObjList = new ArrayList();
				if(insFileHT.containsKey(obj[0].toString().trim())){
					List list = (List) insFileHT.get(obj[0].toString().trim());
					for(int j=0;j<list.size();j++){
						//进行数据抛转时每次都需要判断是否可以保存 by mark
						if(cp.getState()==false)
							continue;
						
						Object[] fileObj = (Object[]) list.get(j);
						
						Object[] objInsFileCenter = new Object[17];
						objInsFileCenter[0] = oid;//fileObj[0];//INSTNO
						if(fileObj[1]!=null){
							objInsFileCenter[1] = fileObj[1].toString().trim();//SRCNAME
						}else{
							objInsFileCenter[1] = "";
						}
						if(fileObj[2]!=null){
							objInsFileCenter[2] = fileObj[2].toString().trim();//SRCCNT
						}else{
							objInsFileCenter[2] = "";
						}
						if(fileObj[3]!=null){
							objInsFileCenter[3] = fileObj[3].toString().trim();//SRCSUM
						}else{
							objInsFileCenter[3] = "";
						}
						objInsFileCenter[4] = "";//SRCOTHER
						objInsFileCenter[5] = "";//CHGNAME
						objInsFileCenter[6] = "";//CHGCNT
						objInsFileCenter[7] = "";//CHGSUM
						objInsFileCenter[8] = "";//CHGOTHER
						
						if(fileObj[5]!=null){
							objInsFileCenter[9] = fileObj[5].toString().trim();//TARNAME
						}else{
							objInsFileCenter[9] = "";//TARNAME
						}
						if(fileObj[6]!=null){
							objInsFileCenter[10] = fileObj[6].toString().trim();//TARCNT
						}else{
							objInsFileCenter[10] = "";//TARCNT
						}
						if(fileObj[7]!=null){
							objInsFileCenter[11] = fileObj[7].toString().trim();//TARSUM
						}else{
							objInsFileCenter[11] = "";//TARSUM
						}
						if(fileObj[8]!=null){
							objInsFileCenter[12] = fileObj[8].toString().trim();//TAROTHER
						}else{
							objInsFileCenter[12] = "";//TAROTHER
						}
						if(fileObj[9]!=null){
							objInsFileCenter[13] = fileObj[9].toString().trim();
							srclib = fileObj[9].toString().trim();
						}else{
							objInsFileCenter[13] = "";//SRCLIB
						}
						if(fileObj[10]!=null){
							objInsFileCenter[14] = fileObj[10].toString().trim();
						}else{
							objInsFileCenter[14] = "";//TEMPNAME
						}
						if(fileObj[11]!=null){
							objInsFileCenter[16] = fileObj[11].toString().trim();//TEMPLIB
						}else{
							objInsFileCenter[16] = "";//TEMPLIB
						}
						if(fileObj[12]!=null){
							objInsFileCenter[15] = fileObj[12].toString().trim();//TARLIB
						}else{
							objInsFileCenter[15] = "";//TARLIB
						}
						
//						sqlList.add(sqlInsFileCenter);
//						objAryList.add(objInsFileCenter);
						
//						sqlListTest.add(sqlInsFileCenterTest);
//						objAryListTest.add(objInsFileCenter);
						
						
						//log.info("srcname="+fileObj[1].toString().trim()+" srcsum="+fileObj[3]);
						//log.info("tarname="+fileObj[5]);
						
						String srcsql = "select * from "+fileObj[9].toString().trim()+"."+fileObj[1].toString().trim()+" where INSTNO = ?";
						Object[] sobj = new Object[1];
						sobj[0] = fileObj[0];
						List srclist = dbo.getData(srcsql,sobj, connectInfo);
						
						objInsFileCenter[6] = srclist.size();//CHGCNT
						objInsFileCenter[10] = srclist.size();//TARCNT
						insFileObjList.add(objInsFileCenter);
						
						
						//log.info("srclist="+srclist.size());
						List columnslist = dbo.getColumnsList();
						String columnnames = "";
						String vals = "";
						int insNoIdx = 0;
						for(int k=0;k<columnslist.size();k++){
							columnnames += columnslist.get(k).toString()+",";
							vals += "?,";
							if(columnslist.get(k).toString().equalsIgnoreCase("INSTNO")){
								insNoIdx = k;
							}
						}
						System.out.println("insNoIdx="+insNoIdx);
						if(!columnnames.equals("")){
							columnnames = columnnames.substring(0,columnnames.length()-1);
							vals = vals.substring(0,vals.length()-1);
						}

						log.info("oid="+oid);
						String tarsql = "insert into "+objInsFileCenter[16].toString().trim()+"."+fileObj[10].toString().trim()+" values("+vals+")";
						String tarsqlTest = "insert into interfacetest."+fileObj[10].toString().trim()+" values("+vals+")";
						List tempAry = new ArrayList();
						
						insNos = insNos + fileObj[0] + ",";
						log.info("srcsql="+srcsql+"  insNo="+fileObj[0]+"  "+srclist.size());
						log.info("tarsql="+tarsql);
//						System.out.println("tarsqlTest="+tarsqlTest);
						for(int m=0;m<srclist.size();m++){
							Object[] srcObj = (Object[]) srclist.get(m);
							for(int n=0;n<srcObj.length;n++){
								if(n==insNoIdx){
									srcObj[n] = oid;
								}else{
									if(srcObj[n]!=null){
										srcObj[n] = srcObj[n].toString().trim();
										if(srcSrv.equals("INTC")){
											if(srcObj[n].equals("")){
												srcObj[n] = " ";
											}
										}
									}else{
										srcObj[n] = "";
									}
								}
							}
							tempAry.add(srcObj);
//							dbo.execData(PropertysUtil.getConnectInfo(), tarsql, srcObj);
						}
						dbo.execDataByTrancation(PropertysUtil.getConnectInfo(), tarsql, tempAry);
//						dbo.execDataByTrancation(PropertysUtil.getConnectInfo(), tarsqlTest, tempAry);
						
						chgcount += srclist.size();//计算CHGCNT
					}
					dbo.execDataByTrancation(PropertysUtil.getConnectInfo(), sqlInsFileCenter, insFileObjList);
//					dbo.execDataByTrancation(PropertysUtil.getConnectInfo(), sqlInsFileCenterTest, insFileObjList);
				}
				//是否可以将标志设为更新成功 by mark
				boolean updateFlag = true;
				//如果有异常发生，就把oid设为空 by mark
				if(cp.isFireException()==true){
					chgCntHT.put(oid, chgcount);
					updateFlag = false;
				}
				//log.info("put data to datacenter start "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
				dbo.execDataByTrancation(PropertysUtil.getConnectInfo(), sqlInsCenter, objAryList);
//				dbo.execDataByTrancation(PropertysUtil.getConnectInfo(), sqlInsCenterTest, objAryList);

				
				//增加一个是否可以更新的判断 by mark
				if(!srclib.equals("") && updateFlag==true){
					System.out.println("srclib is not null = "+obj[0]);
					Object[] updateObj = new Object[2];
					updateObj[0] = "0";
					updateObj[1] = obj[0];
					String updateInsSql = "update "+srclib+".CHGINST set CHGSTS = ? where INSTNO = ?";
					dbo.execData(connectInfo, updateInsSql, updateObj);
					log.info("update chgsts to 0 in "+code);
				}

			}
			Iterator chgCntIT = chgCntHT.keySet().iterator();
			List updatesqllist = new ArrayList();
			
			while(chgCntIT.hasNext()){
				String oid = chgCntIT.next().toString();
				String cnt = chgCntHT.get(oid).toString();
				String sql = "update interface.HOLA_APP_CHG_INST set CHGCNT=? ";
				Object[] obj = null;
				System.out.println("oid="+oid+"   cnt="+cnt);
				if(Integer.parseInt(cnt)==0){
					sql += ",TARSTATUS=?";
					obj = new Object[3];
					obj[0] = cnt;
					obj[1] = "";//1
					obj[2] = oid;
				}else{
					obj = new Object[2];
					obj[0] = cnt;
					obj[1] = oid;
				}
				sql += " where INSTNO=?";
				dbo.execData(PropertysUtil.getConnectInfo(), sql, obj);
			}

			log.info("put data to datacenter from "+code+" end");
			
		}catch(Exception e){
			log.info("putDataToCenter ["+DateUtil.formatDate3(DateUtil.getCurrentDate())+"] error insNo : "+insNos);
			e.printStackTrace();
			//发生异常后需要将chgcode对应的记录更新成异常状态。 by mark
			cp.updateState(0);
		}
	}
	
	public void putData(){
		try{
			String sqlInsCenter = "select INSTNO,SYS_CODE,CHG_CODE,CHG_TYPE,to_char(SRCDATE,'yyyymmddhh24miss'),SRCCNT,SRCUSER,SRCSTATUS,FILECNT,CHGDATE,CHGCNT,CHGUSER,CHGSTATUS,TRGSERVER,TARDATE,TARCNT,TARUSER,TARSTATUS,SRCSERVER,OID from interface.HOLA_APP_CHG_INST where TARSTATUS is null and (TRGSERVER not in ('BANK') or TRGSERVER is null)";
			Object[] objInsCenter = new Object[0];
			List insList = dbo.getData(sqlInsCenter, objInsCenter, PropertysUtil.getConnectInfo());

			log.info("put data to tar start");
			log.info("insList="+insList.size());
			
			String sqlInsFileCenter = "select INSTNO,SRCNAME,SRCCNT,SRCSUM,SRCOTHER,CHGNAME,CHGCNT,CHGSUM,CHGOTHER,TARNAME,TARCNT,TARSUM,TAROTHER,SRCLIB,TEMPNAME,TARLIB,TEMPLIB from interface.HOLA_APP_CHG_INST_FILE where INSTNO in (select INSTNO from interface.HOLA_APP_CHG_INST where TARSTATUS is null and (TRGSERVER not in ('BANK') or TRGSERVER is null))";
			List insFileList = dbo.getData(sqlInsFileCenter, objInsCenter, PropertysUtil.getConnectInfo());
			
			log.info("insFileList="+insFileList.size());
			
			Hashtable codeHT = new Hashtable();
			for(int i=0;i<insFileList.size();i++){
				Object[] obj = (Object[]) insFileList.get(i);
				String insNo = (String) obj[0];//INSTNO
				String code = insNo.substring(0,insNo.length()-8);//Uuid
				Hashtable insNoHT = null;
				if(codeHT.get(code)==null){
					insNoHT = new Hashtable();
				}else{
					insNoHT = (Hashtable) codeHT.get(code);
				}
				List insNoList = null;
				if(insNoHT.get(insNo)==null){
					insNoList = new ArrayList();
				}else{
					insNoList = (List) insNoHT.get(insNo);
				}
				
				insNoList.add(obj);
				insNoHT.put(insNo, insNoList);//根据insNo区分记录
				codeHT.put(code, insNoHT);//根据code区分tar
			}
			System.out.println("codeHT="+codeHT.size());
			Iterator codeIT = codeHT.keySet().iterator();
			while(codeIT.hasNext()){
				String code = codeIT.next().toString();
				Hashtable insNoHT = (Hashtable) codeHT.get(code);
				for(int i=0;i<insList.size();i++){
					Object[] obj = (Object[]) insList.get(i);
					if(insNoHT.containsKey(obj[0].toString())){
						if(obj[18]!=null&&obj[13]!=null){
							String srcSvr = obj[18].toString();//数据来源		SRCSERVER
							String tarSvr = obj[13].toString();//数据目的地	TRGSERVER

//							System.out.println("srcSvr="+srcSvr+"  tarSvr="+tarSvr);
							Hashtable srcSvrConnHT = (Hashtable) DataPool.connInfoHT.get(srcSvr.trim());//数据源连接信息
							Hashtable tarSvrConnHT = (Hashtable) DataPool.connInfoHT.get(tarSvr.trim());//数据目的地连接信息
							
							if(tarSvrConnHT!=null&&tarSvrConnHT.get("InstLib")!=null){
								String tarLib = tarSvrConnHT.get("InstLib").toString();
								List filelist = (List) insNoHT.get(obj[0].toString());
								
								int count = 0;
								int tarcnt = 0;
								List sqllist = new ArrayList();
								List objlist = new ArrayList();
								int totalcnt = 0;
								int totalfilecnt = 0;
								String objtype = "";
								for(int j=0;j<filelist.size();j++){
									Object[] fileobj = (Object[]) filelist.get(j);
//									for(int kk=0;kk<fileobj.length;kk++){
//										System.out.println(code+"=kk="+kk+"= "+fileobj[kk]);
//									}
									tarcnt = 0;
									if(tarSvrConnHT.get("isWrite")!=null&&tarSvrConnHT.get("isWrite").toString().trim().equals("0")){//isWrite 0.写入目的地  1.不写入目的地
										String insNo = obj[0].toString().trim();//OID
										String srcsql = "select * from "+fileobj[16].toString().trim()+"."+fileobj[14].toString().trim()+" where INSTNO='"+fileobj[0]+"'";
										List srclist = dbo.getData(srcsql, new Object[0], PropertysUtil.getConnectInfo());
										System.out.println("scrlist="+srclist.size());
										
										List columnslist = dbo.getColumnsList();
										String columnnames = "";
										String vals = "";
										int insNoIdx = 0;
										for(int k=0;k<columnslist.size();k++){
											columnnames += columnslist.get(k).toString()+",";
											vals += "?,";
											if(columnslist.get(k).toString().equalsIgnoreCase("INSTNO")){
												insNoIdx = k;
											}
										}
										if(!columnnames.equals("")){
											columnnames = columnnames.substring(0,columnnames.length()-1);
											vals = vals.substring(0,vals.length()-1);
										}
										List objAryList = new ArrayList();
										
										for(int ii=0;ii<fileobj.length;ii++){
											System.out.println("fileobj "+ii+"="+fileobj[ii]);
										}
										System.out.println("fileobj[15]="+fileobj[15]+"  fileobj[9]="+fileobj[9]+" vals="+vals);
										
										String tarsql = "insert into "+fileobj[15].toString().trim()+"."+fileobj[9].toString().trim()+" values ("+vals+")";
										System.out.println("srcsql="+srcsql);
										System.out.println("tarsql="+tarsql);
										
										for(int m=0;m<srclist.size();m++){
											Object[] srcObj = (Object[]) srclist.get(m);
											for(int n=0;n<srcObj.length;n++){
												if(n==insNoIdx){
													srcObj[n] = insNo;
//													System.out.println("srcObj["+n+"]="+srcObj[n]);
												}else{
													if(srcObj[n]!=null){
														srcObj[n] = srcObj[n].toString().trim();
														if(srcSvr.equals("INTC")){
															if(srcObj[n].toString().trim().length()==0){
																srcObj[n] = " ";
															}
														}
													}else{
														if(srcSvr.equals("INTC")){
															srcObj[n] = " ";
														}else{
															srcObj[n] = "";
														}
													}
												}
											}
//											System.out.println("srcObj length = "+srcObj.length);
											if(tarsql.indexOf("KGSMART.dbo.HOLA_EC_ODRITEM")!=-1&&tarsql.indexOf("KGSMART.dbo.HOLA_EC_ODRITEM_PROMO")==-1){
												if(srcObj[12].equals("")){
													srcObj[12] = "0";
												}
											}else if(tarsql.indexOf("KGSMART.dbo.HOLA_EC_ODRITEM_PROMO")!=-1){
												if(srcObj[9].equals("")){
													srcObj[9] = "0";
												}
											}else if(tarsql.indexOf("KGSMART.dbo.HOLA_EC_ODRMST")!=-1){
												if(srcObj[26].equals("")){
													srcObj[26] = "0";
												}
												if(srcObj[27].equals("")){
													srcObj[27] = "0";
												}
												if(srcObj[17].equals("")){
													srcObj[17] = "0";
												}
												if(srcObj[21].equals("")){
													srcObj[21] = "0";
												}
											}
											objAryList.add(srcObj);
//											dbo.execData(tarSvrConnHT, tarsql, srcObj);
										}
										
										log.info("start import "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
										dbo.execDataByTrancationForAS400(tarSvrConnHT, tarsql, objAryList);
										log.info("end import "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
										tarcnt += srclist.size();
									}
									
									if(tarSvrConnHT.get("isRead")!=null&&tarSvrConnHT.get("isRead").toString().trim().equals("0")){//isRead 0.有Ins和InsFile需要写入  1.无
										totalcnt += tarcnt;
										totalfilecnt ++;
										
										String sqlInsFile = "insert into "+tarLib+".CHGINSTF (INSTNO,SRCNAM,TARNAM,SRCCNT,SRCSUM,OTHSUM,TARCNT,TARSUM,TAROTH,SRCLIB,TMPNAM,TMPLIB,TARLIB) values (?,?,?,?,?,?,?,?,?,?,?,?,?)";
										Object[] insfileobj = new Object[13];
										insfileobj[0] = obj[0].toString();//INSTNO
										insfileobj[1] = fileobj[1].toString();//SRCNAM
										insfileobj[2] = fileobj[9].toString();//TARNAM
										insfileobj[3] = tarcnt;//SRCCNT	
										insfileobj[4] = tarcnt;//SRCSUM	
										insfileobj[5] = "0";//OTHSUM
										insfileobj[6] = tarcnt;//TARCNT
										insfileobj[7] = tarcnt;//TARSUM
										insfileobj[8] = "0";//TAROTH
										insfileobj[9] = "INTF";//SRCLIB
										insfileobj[10] = fileobj[14].toString();//TMPNAM
										insfileobj[11] = "INTERFACE";//TMPLIB
										insfileobj[12] = tarLib;//TARLIB
										
										sqllist.add(sqlInsFile);
										objlist.add(insfileobj);
										
										if(count ==0){
											objtype = fileobj[1].toString();
//											String sqlIns = "insert into "+tarLib+".CHGINST (INSTNO,CHGSTS,CHGCOD,CHGTYP,SRCDAT,SRCTIM,SRCCNT,SRCUSR,SRCSTS,TRGSVR,FILCNT,CHGDAT,CHGCNT,CHGUSR,OID,PRTCOD) values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
//											Object[] insobj = new Object[16];
//											insobj[0] = obj[0].toString();//INSTNO
//											if(tarSvr.equals("JDA")){
//												insobj[1] = "0";//CHGSTS
//											}else{
//												insobj[1] = "1";//CHGSTS
//											}
//											if(fileobj[1].toString().equals("VOUCHER_HEADER")&&tarSvr.equals("JDA")){
//												insobj[2] = "VCHRCV";//CHGCOD
//											}else if(fileobj[1].toString().equals("VENDOR_GENERAL")&&tarSvr.equals("JDA")){
//												insobj[2] = "VNDIN";//CHGCOD
//											}else if(fileobj[1].toString().equals("VENDOR_GENERAL")&&tarSvr.equals("MSG")){
//												insobj[2] = "SAP_VDR_I";//CHGCOD
//											}else if(fileobj[1].toString().equals("PAYMENT_DATA")&&tarSvr.equals("JDA")){
//												insobj[2] = "PAYRCV";//CHGCOD
//											}else if(fileobj[1].toString().equals("VOUCHER_REV")&&tarSvr.equals("JDA")){
//												insobj[2] = "REVIN";//CHGCOD
//											}else{
//												if(obj[2]!=null){
//													insobj[2] = obj[2];//CHGCOD
//												}else{
//													insobj[2] = "";//CHGCOD
//												}
//											}
//											insobj[3] = "I";//CHGTYP
//											insobj[4] = obj[4].toString().subSequence(0, 8);//SRCDAT 
//											insobj[5] = obj[4].toString().substring(obj[4].toString().length()-6,obj[4].toString().length());//SRCTIM 
//											insobj[6] = tarcnt;//SRCCNT
//											insobj[7] = "sys";//SRCUSR	interface
//											insobj[8] = "1";//SRCSTS
//											insobj[9] = obj[13].toString();//TRGSVR code
//											insobj[10] = "1";//FILCNT
//											insobj[11] = DateUtil.formatDate4(DateUtil.getCurrentDate()).substring(0,8);//CHGDAT 
//											insobj[12] = tarcnt;//CHGCNT	
//											insobj[13] = "sys";//CHGUSR	sys
//											insobj[14] = "1";//OID
//											insobj[15] = "1";//PRTCOD
//											
//											sqllist.add(sqlIns);
//											objlist.add(insobj);
											
										}
										
//										dbo.execDataByTrancationForAS400(tarSvrConnHT, sqllist, objlist);
										count ++;
									}
								}
								if(tarSvrConnHT.get("isRead")!=null&&tarSvrConnHT.get("isRead").toString().trim().equals("0")){//isRead 0.有Ins和InsFile需要写入  1.无
									String sqlIns = "insert into "+tarLib+".CHGINST (INSTNO,CHGSTS,CHGCOD,CHGTYP,SRCDAT,SRCTIM,SRCCNT,SRCUSR,SRCSTS,TRGSVR,FILCNT,CHGDAT,CHGCNT,CHGUSR,OID,PRTCOD) values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
									Object[] insobj = new Object[16];
									insobj[0] = obj[0].toString();//INSTNO
									if(tarSvr.equals("JDA")){
										insobj[1] = "0";//CHGSTS
									}else{
										insobj[1] = "1";//CHGSTS
									}
									if(objtype.equals("VOUCHER_HEADER")&&tarSvr.equals("JDA")){
										insobj[2] = "VCHRCV";//CHGCOD
									}else if(objtype.equals("VENDOR_GENERAL")&&tarSvr.equals("JDA")){
										insobj[2] = "VNDIN";//CHGCOD
									}else if(objtype.equals("VENDOR_GENERAL")&&tarSvr.equals("MSG")){
										insobj[2] = "SAP_VDR_I";//CHGCOD
									}else if(objtype.equals("PAYMENT_DATA")&&tarSvr.equals("JDA")){
										insobj[2] = "PAYRCV";//CHGCOD
									}else if(objtype.equals("RECEIVE_DATA")&&tarSvr.equals("JDA")){
										insobj[2] = "SAP_RCV";//CHGCOD
									}else if(objtype.equals("VOUCHER_REV")&&tarSvr.equals("JDA")){
										insobj[2] = "REVIN";//CHGCOD
									}else{
										if(obj[2]!=null){
											insobj[2] = obj[2];//CHGCOD
										}else{
											insobj[2] = "";//CHGCOD
										}
									}
									insobj[3] = "I";//CHGTYP
									insobj[4] = obj[4].toString().subSequence(0, 8);//SRCDAT 
									insobj[5] = obj[4].toString().substring(obj[4].toString().length()-6,obj[4].toString().length());//SRCTIM 
									insobj[6] = totalcnt;//SRCCNT
									insobj[7] = "sys";//SRCUSR	interface
									insobj[8] = "1";//SRCSTS
									insobj[9] = obj[13].toString();//TRGSVR code
									insobj[10] = totalfilecnt;//FILCNT
									insobj[11] = DateUtil.formatDate4(DateUtil.getCurrentDate()).substring(0,8);//CHGDAT 
									insobj[12] = totalcnt;//CHGCNT	
									insobj[13] = "sys";//CHGUSR	sys
									insobj[14] = "1";//OID
									insobj[15] = "1";//PRTCOD
									
									sqllist.add(sqlIns);
									objlist.add(insobj);
									
									dbo.execDataByTrancationForAS400(tarSvrConnHT, sqllist, objlist);
								}
								
								if(tarSvrConnHT.get("isWrite")!=null&&tarSvrConnHT.get("isWrite").toString().trim().equals("0")){
									String updateInsCenter = "update interface.HOLA_APP_CHG_INST set CHGSTATUS=?,TARDATE=to_date(?,'yyyy-mm-dd hh24:mi:ss'),TARCNT=?,TARUSER=?,TARSTATUS=? where INSTNO = ?";
									Object[] updateobj = new Object[6];
									updateobj[0] = "0";//CHGSTATUS
									updateobj[1] = DateUtil.formatDate3(DateUtil.getCurrentDate());//TARDATE
									updateobj[2] = tarcnt;//TARCNT
									updateobj[3] = "sys";//TARUSER
									updateobj[4] = "1";//TARSTATUS
									updateobj[5] = obj[0];//INSTNO
									dbo.execData(PropertysUtil.getConnectInfo(), updateInsCenter, updateobj);
								}
							}
							
						}
					}
				}
			}
			log.info("put data to tar end");
			
//			System.out.println("insFileList="+insFileList.size());
//			Hashtable insFileHT = new Hashtable();
//			for(int i=0;i<insFileList.size();i++){
//				Object[] obj = (Object[]) insFileList.get(i);
//				String instNo = (String) obj[0];//INSTNO
//				List list = null;
//				if(insFileHT.containsKey(instNo.trim())){
//					list = (List) insFileHT.get(instNo.trim());
//				}else{
//					list = new ArrayList();
//				}
//				list.add(obj);
//				insFileHT.put(instNo.trim(), list);
//			}
//			for(int i=0;i<insList.size();i++){
//				Object[] obj = (Object[]) insList.get(i);
//				if(obj[18]!=null&&obj[13]!=null){
//					String srcSvr = obj[18].toString();//数据来源
//					String tarSvr = obj[13].toString();//数据目的地
//					System.out.println("srcSvr="+srcSvr+"  tarSvr="+tarSvr);
//					
//					Hashtable srcSvrConnHT = (Hashtable) DataPool.connInfoHT.get(srcSvr.trim());//数据源连接信息
//					Hashtable tarSvrConnHT = (Hashtable) DataPool.connInfoHT.get(tarSvr.trim());//数据目的地连接信息
//					
//					if(tarSvrConnHT.get("isWrite")!=null&&tarSvrConnHT.get("isWrite").toString().trim().equals("0")){//isWrite 0.写入目的地  1.不写入目的地
////						System.out.println("insNo:"+obj[0]);//INSNO
//						if(insFileHT.get(obj[0])!=null){
//							List filelist = (List) insFileHT.get(obj[0]);
//							System.out.println("insNo:"+obj[0]+" oid="+obj[19]+" file:"+filelist.size());
//							int tarcnt = 0;
//							for(int j=0;j<filelist.size();j++){
//								Object[] fileobj = (Object[]) filelist.get(j);
//								
//								String insNo = obj[0].toString().trim();//OID
//								String srcsql = "select * from "+fileobj[16].toString().trim()+"."+fileobj[14].toString().trim()+" where INSTNO='"+fileobj[0]+"'";
//								List srclist = dbo.getData(srcsql, new Object[0], PropertysUtil.getConnectInfo());
//								System.out.println("scrlist="+srclist.size());
//								
//								List columnslist = dbo.getColumnsList();
//								String columnnames = "";
//								String vals = "";
//								int insNoIdx = 0;
//								for(int k=0;k<columnslist.size();k++){
//									columnnames += columnslist.get(k).toString()+",";
//									vals += "?,";
//									if(columnslist.get(k).toString().equalsIgnoreCase("INSTNO")){
//										insNoIdx = k;
//									}
//								}
////								System.out.println("insNoIdx="+insNoIdx);
//								if(!columnnames.equals("")){
//									columnnames = columnnames.substring(0,columnnames.length()-1);
//									vals = vals.substring(0,vals.length()-1);
//								}
//								List objAryList = new ArrayList();
////								String tarsql = "insert into INTF."+fileobj[9].toString().trim()+" ("+columnnames+") values ("+vals+")";
////								System.out.println("fileobj[9]="+fileobj[9]);
//								String tarsql = "insert into "+fileobj[15].toString().trim()+"."+fileobj[9].toString().trim()+" values ("+vals+")";
//								System.out.println("srcsql="+srcsql);
//								System.out.println("tarsql="+tarsql);
//								
//								for(int m=0;m<srclist.size();m++){
//									Object[] srcObj = (Object[]) srclist.get(m);
//									for(int n=0;n<srcObj.length;n++){
//										if(n==insNoIdx){
//											srcObj[n] = insNo;
////											System.out.println("srcObj["+n+"]="+srcObj[n]);
//										}else{
//											if(srcObj[n]!=null){
//												srcObj[n] = srcObj[n].toString().trim();
//											}else{
//												srcObj[n] = "";
//											}
//										}
//
//									}
//									objAryList.add(srcObj);
////									dbo.execData(tarSvrConnHT, tarsql, srcObj);
//								}
//								
//								System.out.println("start import "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
//								dbo.execDataByTrancation(tarSvrConnHT, tarsql, objAryList);
//								System.out.println("end import "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
//								tarcnt += srclist.size();
//							}
//							
//							String updateInsCenter = "update interface.HOLA_APP_CHG_INST set CHGSTATUS=?,TARDATE=to_date(?,'yyyy-mm-dd hh24:mi:ss'),TARCNT=?,TARUSER=?,TARSTATUS=? where INSTNO = ?";
//							Object[] updateobj = new Object[6];
//							updateobj[0] = "0";//CHGSTATUS
//							updateobj[1] = DateUtil.formatDate3(DateUtil.getCurrentDate());//TARDATE
//							updateobj[2] = tarcnt;//TARCNT
//							updateobj[3] = "sys";//TARUSER
//							updateobj[4] = "1";//TARSTATUS
//							updateobj[5] = obj[0];//INSTNO
//							dbo.execData(PropertysUtil.getConnectInfo(), updateInsCenter, updateobj);
//						}
//					}else{
//						
//					}
//				}
//			}
		}catch(Exception e){
			e.printStackTrace();
		}
	}
	
	
	
	public void run() throws Exception{
		List list = DataPool.connInfoList;
		for(int i=0;i<list.size();i++){
			Hashtable ht = (Hashtable) list.get(i);
			if(ht.get("isRead")!=null&&ht.get("isRead").toString().equals("0")){//isRead 0.读取INST  1不读取INST
				this.getData(ht);
			}
		}
		this.putData();
	}
	
	public void execute(JobExecutionContext context) throws JobExecutionException {
		try {
			SAPBean sapBean = new SAPBean();
			sapBean.run();
			
			this.run();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	public static void main(String[] args){
		FileExchange file = new FileExchange();
		PropertysUtil pu = new PropertysUtil();
		try {
			pu.initPropertysUtil();
			pu.getDBConnInfo();
			file.run();
			
//			Runtime lRuntime = Runtime.getRuntime();
//			System.out.println("*** BEGIN MEMORY STATISTICS ***<br/>");
//			System.out.println("Free  Memory: "+lRuntime.freeMemory()+"<br/>");
//			System.out.println("Max   Memory: "+lRuntime.maxMemory()+"<br/>");
//			System.out.println("Total Memory: "+lRuntime.totalMemory()+"<br/>");
//			System.out.println("Available Processors : "+lRuntime.availableProcessors()+"<br/>");
//			System.out.println("*** END MEMORY STATISTICS ***");
			
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
}
