package com.hola.bs.core.schedule;

import java.net.InetAddress;
import java.net.UnknownHostException;
import java.util.List;
import java.util.Map;

import com.hola.bs.property.ConfigPropertyUtil;
import com.hola.bs.property.SQLPropertyUtil;
import com.hola.bs.util.CommonStaticUtil;
import com.hola.bs.util.SpringUtil;

/**
 * 定时任务，定时上传数据至MQ
 * @author S2139
 * 2012 Nov 2, 2012 10:48:34 AM 
 */
public class UploadToMqTask extends java.util.TimerTask{
	
	public SQLPropertyUtil sqlpropertyUtil = new SQLPropertyUtil();
	private ConfigPropertyUtil c = new ConfigPropertyUtil();
	public void run(){
		System.out.println("开始运行MQ任务！");
		try{
			String bridgeServerIp = InetAddress.getLocalHost().getHostAddress().toString();
			List<Map> storeList = SpringUtil.searchForList(sqlpropertyUtil.getValue(null, "hhtstore.01"), new Object[]{bridgeServerIp});
			if(storeList!=null && storeList.size()>0){
				//把每个店铺当作一个SCHEMA，检索未上传的批次号
				for(Map m : storeList){
					String sto_no = m.get("sto_no").toString();
					List<Map> instnoList = SpringUtil.searchForList(sqlpropertyUtil.getValue(sto_no, "hht.instno.list"));
					if(instnoList!=null && instnoList.size()>0){
						//创建一个线程对象，上传数据到MQ
						String[] instnoArray = CommonStaticUtil.tranListMapToArray(instnoList, "instno");
						String sql = "update hht"+sto_no+".chginst set chgsts = '9' where ";
						sql = sql + getWhereCondition(instnoArray);
						SpringUtil.update(sql);
						Thread thread = new SendMqThread(sto_no, instnoArray);
						thread.start();
						//执行完RUN方法会自动关闭？
					}
				}
			}
		}catch(UnknownHostException e){
			e.printStackTrace();
		}
		
	}
	
	private String getWhereCondition(String[] instnoArray){
		StringBuffer sb = new StringBuffer();
		int i =0;
		for(String instno : instnoArray){
			sb.append("instno = '").append(instno).append("'");
			if(i != instnoArray.length-1){
				sb.append(" or ");
			}
			i++;
		}
		return sb.toString();
	}
	
	public static void main(String[] args) throws UnknownHostException{
		
		System.out.println(InetAddress.getLocalHost().getHostAddress().toString());
	}
}
