package com.task.bean;

import java.util.ArrayList;
import java.util.Hashtable;
import java.util.Iterator;
import java.util.List;

import com.task.common.DataPool;
import com.task.common.DateUtil;
import com.task.common.PropertysUtil;
import com.task.common.StringUtil;
import com.task.common.Uuid16;
import com.task.db.DBOperator;

public class SAPBean {

	private Hashtable connectInfoSAP = (Hashtable) DataPool.connInfoHT.get("SAP");
	
	private Hashtable connectInfoAS400 = (Hashtable) DataPool.connInfoHT.get("JDA");
	
	private DBOperator dbo = new DBOperator();
	
	/**
	 * 
	 * @param srcName	VOUCHER_HEADER,VENDOR_GENERAL,BANK_PAYMENT,PAYMENT_DATA,COPA_ROWDATA
	 * @return
	 */
	public Hashtable getData(String srcName){
		Hashtable ht = new Hashtable();
		try{
			String sqlHeader = "select * from INTF."+srcName+" where ZSTATUS=?";
			
			Object[] obj = new Object[1];
			obj[0] = "5";
			if(srcName.equals("COPA_ROWDATA"))
			{
				sqlHeader = "select * from INTF."+srcName+" where STATUS is null";
				obj = new Object[0];
			}
			else if(srcName.equals("RECEIVE_DATA"))
			{
				sqlHeader = "select * from INTF."+srcName+" where STATUS=?";
			}
			
			System.out.println("get 1 = "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
			List list = dbo.getData(sqlHeader, obj, connectInfoSAP);
			List columnslist = dbo.getColumnsList();
			System.out.println("get 2 = "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
			
			Hashtable dataHT = new Hashtable();
			Iterator it = dataHT.keySet().iterator();
			
			if(srcName.equals("COPA_ROWDATA")){
				columnslist.add("INSTNO");
			}
			int insNoIdx = 0;
			for(int k=0;k<columnslist.size();k++){
				if(columnslist.get(k).toString().equalsIgnoreCase("INSTNO")){
					insNoIdx = k;
				}
			}

			if(srcName.equals("PAYMENT_DATA")||srcName.equals("RECEIVE_DATA"))
			{
				for(int i=0;i<list.size();i++){
					Object[] tempobj = (Object[]) list.get(i);
					String tarName = "JDA";
					List templist = null;
					if(dataHT.get(tarName)==null){
						templist = new ArrayList();
					}else{
						templist = (List) dataHT.get(tarName);
					}
					templist.add(tempobj);
					dataHT.put(tarName, templist);
				}
			}
			else if(srcName.equals("BANK_PAYMENT"))
			{
				dataHT.put("BANK", list);
			}
			else if(srcName.equals("COPA_ROWDATA"))
			{
				for(int i=0;i<list.size();i++){
					Object[] tempobj = (Object[]) list.get(i);
					String tarName = "MSG";
					List templist = null;
					if(dataHT.get(tarName)==null){
						templist = new ArrayList();
					}else{
						templist = (List) dataHT.get(tarName);
					}
					templist.add(tempobj);
					dataHT.put(tarName, templist);
				}
			}
			else
			{
				for(int i=0;i<list.size();i++){
					Object[] tempobj = (Object[]) list.get(i);
					
					String tarName = tempobj[insNoIdx].toString().substring(0,3);//tempobj[insNoIdx].toString().substring(0,tempobj[insNoIdx].toString().length()-8);//Uuid
					System.out.println("insNoIdx="+insNoIdx+"  instno="+ tempobj[insNoIdx].toString()+"  tarName="+tarName+" ="+ tempobj[insNoIdx].toString().substring(0,3));
					
					List templist = null;
					if(dataHT.get(tarName)==null){
						templist = new ArrayList();
					}else{
						templist = (List) dataHT.get(tarName);
					}
					templist.add(tempobj);
					dataHT.put(tarName, templist);
				}
			}
			System.out.println("get 3 = "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
			List updateAry = new ArrayList();
			String sqlHeaderUpdate = "";
			if(srcName.equals("RECEIVE_DATA"))
			{
				sqlHeaderUpdate = "update INTF."+srcName+" set STATUS=? where ZELNR=?";
			}
			else if(srcName.equals("COPA_ROWDATA"))
			{
				sqlHeaderUpdate = "update INTF."+srcName+" set STATUS=? where ZELNR=? and BUKRS=? and ERDAT=?";				
			}
			else
			{
				sqlHeaderUpdate = "update INTF."+srcName+" set ZSTATUS=? where ZELNR=?";
			}
			Hashtable ZELNRHT = new Hashtable();
			List testlist = new ArrayList();
			for(int i=0;i<list.size();i++){
				Object[] tempobj = (Object[]) list.get(i);
				if(!srcName.equals("COPA_ROWDATA")){
					ZELNRHT.put(tempobj[0].toString(), tempobj[0].toString());
				}else{
					ZELNRHT.put(tempobj[0].toString()+";"+tempobj[1].toString()+";"+tempobj[2].toString(), tempobj[0].toString());
				}
			}
			System.out.println("get 4 = "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
			Iterator ZELNRIT = ZELNRHT.keySet().iterator();
			while(ZELNRIT.hasNext()){
				String ZELNR = ZELNRIT.next().toString();
				Object[] updateobj = new Object[2];
				if(srcName.equals("COPA_ROWDATA")){
					updateobj = new Object[4];
					updateobj[0] = "A";
					updateobj[1] = ZELNR.split(";")[0];
					updateobj[2] = ZELNR.split(";")[1];
					updateobj[3] = ZELNR.split(";")[2];
				}else{
					updateobj[0] = "A";
					updateobj[1] = ZELNR;
				}
				updateAry.add(updateobj);
			}
			System.out.println("get 5 = "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
			dbo.execDataByTrancation(connectInfoSAP, sqlHeaderUpdate, updateAry);
			System.out.println("get 6 = "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
			
			ht.put("resultHT", dataHT);
			ht.put("result", list);
			ht.put("columns", columnslist);
			System.out.println("========getDataEnd=======");
		}catch(Exception e){
			e.printStackTrace();
		}
		return ht;
	}
	
	/**
	 * 
	 * @param resultlist
	 * @param columnslist
	 * @param tempName	
	 * HOLA_APP_SAP_VENDOR_GENERAL
	 * HOLA_APP_SAP_VOUCHER_HEADER
	 * HOLA_APP_SAP_BANK_PAYMENT
	 * HOLA_APP_SAP_PAYMENT_DATA
	 * @return
	 */
	public String putHeaderDataToCenter(Hashtable resultHT,List columnslist,String srcName,String tarName,String tempName){
//		List resultlist = null;
		String oid = "";
		try{
			
			Iterator it = resultHT.keySet().iterator();
			while(it.hasNext()){
				String code = it.next().toString();
				List resultlist = (List) resultHT.get(code);
				System.out.println("code="+code);
				Hashtable connInfoHT = (Hashtable) DataPool.connInfoHT.get(code);
				
				System.out.println("putHeaderDataToCenter  connInfoHT="+connInfoHT+"  code="+code);
				
				String insLib = "";
				if(code.equals("BANK")){
					insLib = "";
				}else{
					insLib = connInfoHT.get("InstLib").toString();
				}
				
				System.out.println("insLib = "+insLib);
				
				String vals = "";
				int insNoIdx = 0;
				List sqllist = new ArrayList();
				List objlist = new ArrayList();
				oid =  java.util.UUID.randomUUID().toString().split("-")[0];//Uuid16.create().toString();
				oid = code + oid ;
				
				String sqlIns = "insert into interface.HOLA_APP_CHG_INST (INSTNO,SYS_CODE,CHG_CODE,CHG_TYPE,SRCDATE,SRCCNT,SRCUSER,SRCSTATUS,FILECNT,CHGDATE,CHGCNT,CHGUSER,CHGSTATUS,TRGSERVER,TARSTATUS,OID,SRCSERVER) values (?,?,?,?,to_date(?,'yyyy-mm-dd hh24:mi:ss'),?,?,?,?,to_date(?,'yyyy-mm-dd hh24:mi:ss'),?,?,?,?,?,?,?)";
				Object[] insobj = new Object[17];
				insobj[0] = oid;//INSTNO
				insobj[1] = "SAP";//SYS_CODE
				insobj[2] = "SAP";//CHG_CODE
				insobj[3] = "I";//CHG_TYPE
				insobj[4] = DateUtil.formatDate3(DateUtil.getCurrentDate());//SRCDATE 
				insobj[5] = resultlist.size();//SRCCNT
				insobj[6] = "interface";//SRCUSER
				insobj[7] = "1";//SRCSTATUS
				insobj[8] = "0";//FILECNT
				insobj[9] = DateUtil.formatDate3(DateUtil.getCurrentDate());//CHGDATE
				insobj[10] = "0";//CHGCNT
				insobj[11] = "interface";//CHGUSER
				insobj[12] = "1";//CHGSTATUS
				insobj[13] = code;//TRGSERVER
				insobj[14] = "";//TARSTATUS
				insobj[15] = "";//OID
				insobj[16] = "SAP";//SRCSERVER
				
				sqllist.add(sqlIns);
				objlist.add(insobj);
				
				String sqlInsFile = "insert into interface.HOLA_APP_CHG_INST_FILE (INSTNO,SRCNAME,SRCCNT,SRCSUM,SRCOTHER,CHGNAME,CHGCNT,CHGSUM,CHGOTHER,TARNAME,TARCNT,TARSUM,TAROTHER,SRCLIB,TEMPNAME,TARLIB,TEMPLIB) values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
				Object[] insfileobj = new Object[17];
				
				insfileobj[0] = oid;//INSTNO
				insfileobj[1] = srcName;//SRCNAME
				insfileobj[2] = resultlist.size();//SRCCNT
				insfileobj[3] = "0";//SRCSUM
				insfileobj[4] = "0";//SRCOTHER
				insfileobj[5] = "0";//CHGNAME
				insfileobj[6] = "0";//CHGCNT
				insfileobj[7] = "0";//CHGSUM
				insfileobj[8] = "0";//CHGOTHER
				
				if(code.equals("JDA")&&tarName.equals("VCHINF")){
					insfileobj[9] = "VCHINF";//TARNAME
				}else if(code.equals("MSG")&&tarName.equals("VCHINF")){
					insfileobj[9] = "HOLA_APP_SAP_VOUCHER_HEADER";//TARNAME
				}
				
				if(code.equals("JDA")&&tarName.equals("VNDWKM")){
					insfileobj[9] = "VNDINM";//TARNAME
				}else if(code.equals("MSG")&&tarName.equals("VNDWKM")){
					insfileobj[9] = "HOLA_APP_SAP_VENDOR_GENERAL";//TARNAME
				}
				
				if(code.equals("JDA")&&tarName.equals("SAPPAYF")){
					insfileobj[9] = "SAPPAYF";//TARNAME
				}else if(code.equals("MSG")&&tarName.equals("SAPPAYF")){
					insfileobj[9] = "HOLA_APP_SAP_PAYMENT_DATA";//TARNAME
				}
				
				if(code.equals("JDA")&&tarName.equals("SAPRCVF")){
					insfileobj[9] = "SAPRCVF";//TARNAME
				}
				
				if(code.equals("JDA")&&tarName.equals("REVINF")){
					insfileobj[9] = "REVINF";//TARNAME
				}else if(code.equals("MSG")&&tarName.equals("REVINF")){
					insfileobj[9] = "HOLA_APP_SAP_VOUCHER_REV";//TARNAME
				}
				
				if(tarName.equals("HOLA_APP_SAP_BANK_PAYMENT")){
					insfileobj[9] = "HOLA_APP_SAP_BANK_PAYMENT";//TARNAME
				}
				if(srcName.equals("COPA_ROWDATA")){
					insfileobj[9] = "HOLA_APP_SAP_COPA_ROWDATA";//TARNAME
				}
				
//				insfileobj[9] = tarName;//TARNAME
				insfileobj[10] = "0";//TARCNT
				insfileobj[11] = "0";//TARSUM
				insfileobj[12] = "0";//TAROTHER
				insfileobj[13] = "INTF";//SRCLIB
				insfileobj[14] = tempName;//TEMPNAME
				insfileobj[15] = insLib;//TARLIB
				insfileobj[16] = "INTERFACE";//TEMPLIB
				
				sqllist.add(sqlInsFile);
				objlist.add(insfileobj);
				
				System.out.println("putto Center 1= "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
				dbo.execDataByTrancation(PropertysUtil.getConnectInfo(), sqllist, objlist);
				System.out.println("putto Center 2= "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
				
				for(int k=0;k<columnslist.size();k++){
					vals += "?,";
					if(columnslist.get(k).toString().equalsIgnoreCase("INSTNO")){
						insNoIdx = k;
					}
				}
				if(!vals.equals("")){
					vals = vals.substring(0,vals.length()-1);
				}
				System.out.println("putto Center 3= "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
				
				String sqlHeader = "insert into interface."+tempName+" values ("+vals+")";
				List tempAry = new ArrayList();
				for(int i=0;i<resultlist.size();i++){
					Object[] tempobj = (Object[]) resultlist.get(i);
					Object[] copyobj = new Object[6];
					if(i/100==0){
						System.out.println("index = "+i+"  = "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
					}
					if(srcName.equals("COPA_ROWDATA")){
						copyobj[0] = tempobj[0];
						copyobj[1] = tempobj[1];
						copyobj[2] = tempobj[2];
						if(tempobj[3]!=null){
							copyobj[3] = StringUtil.rightPad(tempobj[3].toString());
						}else{
							copyobj[3] = "";
						}
						copyobj[4] = tempobj[4];
						copyobj[5] = "";
						tempobj = copyobj;
					}
					
					for(int j=0;j<tempobj.length;j++){
						if(j==insNoIdx){
							insobj[15] = tempobj[j];
							tempobj[j] = oid;
						}else{
							if(tempobj[j]!=null){
								tempobj[j] = tempobj[j].toString().trim();
							}else{
								tempobj[j] = "";
							}
						}
//						if(tempobj[j]!=null){
//							System.out.println("index="+j+"  len="+tempobj[j].toString().length()+"  val="+tempobj[j]);
//						}else{
//							System.out.println("index="+j+"  len=0"+"  val="+tempobj[j]);
//						}
					}
					tempAry.add(tempobj);
				}
				System.out.println("putto Center 4= "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
				dbo.execDataByTrancation(PropertysUtil.getConnectInfo(), sqlHeader, tempAry);
				System.out.println("putto Center 5= "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
			}
		}catch(Exception e){
			e.printStackTrace();
		}
		return oid;
	}
	
	public void putHeaderDataToHistory(List list,List columnslist,String srcName,String historyName){
		try{
			String vals = "";
			int zstatusIdx = 0;
			
			for(int k=0;k<columnslist.size();k++){
				vals += "?,";
				if(columnslist.get(k).toString().equalsIgnoreCase("ZSTATUS")){
					zstatusIdx = k;
				}
			}
			if(!vals.equals("")){
				vals = vals.substring(0,vals.length()-1);
			}
			String sql = "insert into INTF."+historyName+" values ("+vals+")";
			List objlist = new ArrayList();
			String insNos = "";
			for(int i=0;i<list.size();i++){
				Object[] tempobj = (Object[]) list.get(i);
				insNos = insNos+"'"+tempobj[0].toString().trim()+"',";
				for(int j=0;j<tempobj.length;j++){
					if(j==zstatusIdx){
						tempobj[j] = "5";
					}else{
						if(tempobj[j]!=null){
							tempobj[j] = tempobj[j].toString().trim();
						}else{
							tempobj[j] = "";
						}
					}
				}
				objlist.add(tempobj);
			}
			dbo.execDataByTrancation(connectInfoSAP, sql, objlist);
			
			if(!insNos.trim().equals("")){
				insNos = insNos.substring(0,insNos.length()-1);
			}
			String sqldel = "delete from INTF."+srcName+" where ZELNR in ("+insNos+")";
//			dbo.execData(connectInfo, sqldel, new Object[0]);
			
		}catch(Exception e){
			e.printStackTrace();
		}
	}
	
	public void putHeaderDataToTarget(Hashtable resultHT,List columnslist,String oid,String srcName,String tarName,String tempName){
		try{
			
			Iterator it = resultHT.keySet().iterator();
			while(it.hasNext()){
				String code = it.next().toString();
				List list = (List) resultHT.get(code);
				Hashtable connInfoHT = (Hashtable) DataPool.connInfoHT.get(code);
				String insLib = connInfoHT.get("InstLib").toString();
				System.out.println("insLib = "+insLib);
				
				List sqllist = new ArrayList();
				List objlist = new ArrayList();
				
				String sqlIns = "insert into "+insLib+".CHGINST (INSTNO,CHGSTS,CHGCOD,CHGTYP,SRCDAT,SRCTIM,SRCCNT,SRCUSR,SRCSTS,TRGSVR,FILCNT,CHGDAT,CHGCNT,CHGUSR,OID,PRTCOD) values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
				
				Object[] insobj = new Object[16];
				insobj[0] = oid;//INSTNO
				insobj[1] = "1";//CHGSTS
				if(srcName.equals("VOUCHER_HEADER")){
					insobj[2] = "VCHRCV";//CHGCOD
				}else{
					insobj[2] = "SAP";//CHGCOD
				}
				insobj[3] = "I";//CHGTYP
				insobj[4] = "1";//SRCDAT
				insobj[5] = "1";//SRCTIM
				insobj[6] = list.size();//SRCCNT
				insobj[7] = "1";//SRCUSR
				insobj[8] = "0";//SRCSTS
				insobj[9] = code;//TRGSVR
				insobj[10] = "1";//FILCNT
				insobj[11] = "1";//CHGDAT
				insobj[12] = "1";//CHGCNT
				insobj[13] = "1";//CHGUSR
				insobj[14] = "1";//OID
				insobj[15] = "1";//PRTCOD
				
				sqllist.add(sqlIns);
				objlist.add(insobj);
				
				String sqlInsFile = "insert into "+insLib+".CHGINSTF (INSTNO,SRCNAM,TARNAM,SRCCNT,SRCSUM,OTHSUM,TARCNT,TARSUM,TAROTH,SRCLIB,TMPNAM,TMPLIB,TARLIB) values (?,?,?,?,?,?,?,?,?,?,?,?,?)";
				Object[] insfileobj = new Object[13];
				insfileobj[0] = oid;//INSTNO
				insfileobj[1] = srcName;//SRCNAM
				insfileobj[2] = tarName;//TARNAM
				insfileobj[3] = list.size();//SRCCNT
				insfileobj[4] = "0";//SRCSUM
				insfileobj[5] = "0";//OTHSUM
				insfileobj[6] = "0";//TARCNT
				insfileobj[7] = "0";//TARSUM
				insfileobj[8] = "0";//TAROTH
				insfileobj[9] = "INTF";//SRCLIB
				insfileobj[10] = tempName;//TMPNAM
				insfileobj[11] = "INTERFACE";//TMPLIB
				insfileobj[12] = insLib;//TARLIB
				
				sqllist.add(sqlInsFile);
				objlist.add(insfileobj);
				
				dbo.execDataByTrancationForAS400(connInfoHT, sqllist, objlist);
				
				System.out.println("tar inssql="+sqlIns);
				System.out.println("tar insFilesql="+sqlInsFile);
				
				if(code.equals("JDA")&&tarName.equals("VCHINF")){
					tarName = "VCHINF";//TARNAME
				}else if(code.equals("MSG")&&tarName.equals("VCHINF")){
					tarName = "HOLA_APP_SAP_VOUCHER_HEADER";//TARNAME
				}
				
				if(code.equals("JDA")&&tarName.equals("VNDWKM")){
					tarName = "VNDINM";//"VNDINM";//TARNAME
				}else if(code.equals("MSG")&&tarName.equals("VNDWKM")){
					tarName = "HOLA_APP_SAP_VENDOR_GENERAL";//TARNAME
				}
				
				if(code.equals("JDA")&&tarName.equals("SAPPAYF")){
					tarName = "SAPPAYF";//TARNAME
				}else if(code.equals("MSG")&&tarName.equals("SAPPAYF")){
					tarName = "HOLA_APP_SAP_PAYMENT_DATA";//TARNAME
				}
				
				if(code.equals("JDA")&&tarName.equals("REVINF")){
					tarName = "REVINF";//TARNAME
				}else if(code.equals("MSG")&&tarName.equals("REVINF")){
					tarName = "HOLA_APP_SAP_VOUCHER_REV";//TARNAME
				}
				
				String vals = "";
				int insNoIdx = 0;
				for(int k=0;k<columnslist.size();k++){
					vals += "?,";
					if(columnslist.get(k).toString().equalsIgnoreCase("INSTNO")){
						insNoIdx = k;
					}
				}
				if(!vals.equals("")){
					vals = vals.substring(0,vals.length()-1);
				}
				
				String sqlTar = "insert into "+insLib+"."+tarName+" values ("+vals+")";
				
				System.out.println("sqlTar="+sqlTar);
				
				List tempAry = new ArrayList();
				for(int i=0;i<list.size();i++){
					Object[] tempobj = (Object[]) list.get(i);
					for(int j=0;j<tempobj.length;j++){
						if(j==insNoIdx){
							tempobj[j] = oid;
						}else{
							if(tempobj[j]!=null){
								tempobj[j] = tempobj[j].toString();
							}else{
								tempobj[j] = " ";
							}
						}
					}
					tempAry.add(tempobj);
//					dbo.execData(connInfoHT, sqlTar, tempobj);
				}
				dbo.execDataByTrancationForAS400(connInfoHT, sqlTar, tempAry);
			}
		}catch(Exception e){
			e.printStackTrace();
		}
	}
	
	public void putHeader(){
		Hashtable headerSRCHT = this.getData("VOUCHER_HEADER");
		List headerResultList = (List) headerSRCHT.get("result");
		List headerColumnsList = (List) headerSRCHT.get("columns");
		Hashtable headerResultHT = (Hashtable) headerSRCHT.get("resultHT");
		
		String headerOid = this.putHeaderDataToCenter(headerResultHT, headerColumnsList, "VOUCHER_HEADER", "VCHINF", "HOLA_APP_SAP_VOUCHER_HEADER");
		System.out.println("headerOid="+headerOid);
//		this.putHeaderDataToHistory(headerResultList, headerColumnsList, "VOUCHER_HEADER", "VOUCHER_HEADER_H");
//		this.putHeaderDataToTarget(headerResultHT, headerColumnsList, headerOid, "VOUCHER_HEADER", "VCHINF", "HOLA_APP_SAP_VOUCHER_HEADER");
		
	}
	
	public void putGeneral(){
		Hashtable generalSRCHT = this.getData("VENDOR_GENERAL");
		List generalResultList = (List) generalSRCHT.get("result");
		List generalColumnsList = (List) generalSRCHT.get("columns");
		Hashtable generalResultHT = (Hashtable) generalSRCHT.get("resultHT");
		
		String generalOid = this.putHeaderDataToCenter(generalResultHT, generalColumnsList, "VENDOR_GENERAL", "VNDWKM", "HOLA_APP_SAP_VENDOR_GENERAL");
		System.out.println("generalOid="+generalOid);
//		this.putHeaderDataToHistory(generalResultList, generalColumnsList, "VENDOR_GENERAL", "VENDOR_GENERAL_H");
//		this.putHeaderDataToTarget(generalResultHT, generalColumnsList, generalOid, "VENDOR_GENERAL", "VNDWKM", "HOLA_APP_SAP_VENDOR_GENERAL");
		
	}
	
	public void putPayment(){
		Hashtable paymentSRCHT = this.getData("PAYMENT_DATA");
		List paymentResultList = (List) paymentSRCHT.get("result");
		List paymentColumnsList = (List) paymentSRCHT.get("columns");
		Hashtable paymentResultHT = (Hashtable) paymentSRCHT.get("resultHT");
		
		String paymentOid = this.putHeaderDataToCenter(paymentResultHT, paymentColumnsList, "PAYMENT_DATA", "SAPPAYF", "HOLA_APP_SAP_PAYMENT_DATA");
		System.out.println("paymentOid="+paymentOid);
//		this.putHeaderDataToHistory(paymentResultList, paymentColumnsList, "PAYMENT_DATA", "PAYMENT_DATA_H");
//		this.putHeaderDataToTarget(paymentResultHT, paymentColumnsList, paymentOid, "PAYMENT_DATA", "SAPPAYF", "HOLA_APP_SAP_PAYMENT_DATA");
		
	}
	
	public void putRevMent(){
		Hashtable paymentSRCHT = this.getData("RECEIVE_DATA");
		List paymentResultList = (List) paymentSRCHT.get("result");
		List paymentColumnsList = (List) paymentSRCHT.get("columns");
		Hashtable paymentResultHT = (Hashtable) paymentSRCHT.get("resultHT");
		
		String paymentOid = this.putHeaderDataToCenter(paymentResultHT, paymentColumnsList, "RECEIVE_DATA", "SAPRCVF", "HOLA_APP_SAP_RECEIVE_DATA");
		System.out.println("paymentOid="+paymentOid);
//		this.putHeaderDataToHistory(paymentResultList, paymentColumnsList, "PAYMENT_DATA", "PAYMENT_DATA_H");
//		this.putHeaderDataToTarget(paymentResultHT, paymentColumnsList, paymentOid, "PAYMENT_DATA", "SAPPAYF", "HOLA_APP_SAP_PAYMENT_DATA");
		
	}
	
	public void putBankPayment(){
		Hashtable bankpaymentSRCHT = this.getData("BANK_PAYMENT");
		List bankpaymentResultList = (List) bankpaymentSRCHT.get("result");
		List bankpaymentColumnsList = (List) bankpaymentSRCHT.get("columns");
		Hashtable bankpaymentResultHT = (Hashtable) bankpaymentSRCHT.get("resultHT");
		System.out.println("start inert into DataCenter");
		String bankpaymentOid = this.putHeaderDataToCenter(bankpaymentResultHT, bankpaymentColumnsList, "BANK_PAYMENT", "HOLA_APP_SAP_BANK_PAYMENT", "HOLA_APP_SAP_BANK_PAYMENT");
		System.out.println("bankpaymentOid="+bankpaymentOid);
//		this.putHeaderDataToHistory(bankpaymentResultList, bankpaymentColumnsList, "BANK_PAYMENT", "BANK_PAYMENT_H");
	}
	
	public void putREV(){
		Hashtable revSRCHT = this.getData("VOUCHER_REV");
		List revResultList = (List) revSRCHT.get("result");
		List revColumnsList = (List) revSRCHT.get("columns");
		Hashtable revResultHT = (Hashtable) revSRCHT.get("resultHT");
		
		String revOid = this.putHeaderDataToCenter(revResultHT, revColumnsList, "VOUCHER_REV", "REVINF", "HOLA_APP_SAP_VOUCHER_REV");
		System.out.println("revOid="+revOid);
//		this.putHeaderDataToHistory(revResultList, revColumnsList, "VOUCHER_REV", "VOUCHER_REV_H");
//		this.putHeaderDataToTarget(revResultHT, revColumnsList, revOid, "VOUCHER_REV", "REVINF", "HOLA_APP_SAP_VOUCHER_REV");
	}
	
	public void putCopyData(){
		Hashtable copySRCHT = this.getData("COPA_ROWDATA");
		List copyResultList = (List) copySRCHT.get("result");
		List copyColumnsList = (List) copySRCHT.get("columns");
		Hashtable copyResultHT = (Hashtable) copySRCHT.get("resultHT");
		
		String copyOid = this.putHeaderDataToCenter(copyResultHT, copyColumnsList, "COPA_ROWDATA", "HOLA_APP_SAP_COPA_ROWDATA", "HOLA_APP_SAP_COPA_ROWDATA");
//		this.putHeaderDataToTarget(copyResultHT, copyColumnsList, copyOid, "COPA_ROWDATA", "HOLA_APP_SAP_COPA_ROWDATA", "HOLA_APP_SAP_COPA_ROWDATA");
		
		System.out.println("copyOid="+copyOid);
	}
	
	public void run(){
		System.out.println("start SAP To AS400");
		
		System.out.println("SAP TO AS400 Header Start ");
		this.putHeader();
		System.out.println("SAP TO AS400 Header End ");
		
		System.out.println("SAP TO AS400 General Start ");
		this.putGeneral();
		System.out.println("SAP TO AS400 General End ");
		
		System.out.println("SAP TO AS400 Payment Start ");
		this.putPayment();
		System.out.println("SAP TO AS400 Payment End ");
		
		System.out.println("SAP TO AS400 RevMent Start ");
		this.putRevMent();
		System.out.println("SAP TO AS400 RevMent End ");
		
		System.out.println("SAP TO AS400 BankPayment Start ");
		this.putBankPayment();
		System.out.println("SAP TO AS400 BankPayment End ");
		
		System.out.println("SAP TO AS400 REV Start ");
		this.putREV();
		System.out.println("SAP TO AS400 REV End ");
		
//		System.out.println("SAP TO AS400 COPY Start ");
//		this.putCopyData();
//		System.out.println("SAP TO AS400 COPY End ");
		
		System.out.println("end SAP To AS400");
	}
	
	public static void main(String[] args){
		
		PropertysUtil pu = new PropertysUtil();
		pu.initPropertysUtil();
		pu.getDBConnInfo();
		SAPBean sapBean = new SAPBean();
		sapBean.run();
		
	}
	
}
