package com.hola.common.util;

import java.util.Date;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import com.hola.common.exception.FileNameRepeat;

public class CommonUtil 
{
	private		static		CommonUtil		cu = null;
	private		String		currentChgName = "";
	private static Log log = LogFactory.getLog(CommonUtil.class);
	private		static		int			chgnameIndex = 0;
	
	private CommonUtil()
	{
	}
	
	public synchronized static CommonUtil getInstance()
	{
		if(cu == null)
			cu = new CommonUtil();
		return cu;
	}
	
	public static Integer getIntegerForObj(Object obj)
	{
		if(obj instanceof Integer)
			return (Integer) obj;
		if(obj instanceof Long)
		{
			int value = ((Long)obj).intValue();
			return new Integer(value);
		}
		return null;
	}
	public static Integer laycodeToLayno(String srcLayCode) 
	{
		return new Integer(srcLayCode.substring(1, 2));
	}

	public synchronized String getChgName(String msgcode, String storeNo) 
	{
		
		String chgname = msgcode + storeNo + 
							"_" + TimeUtil.formatDate(new Date() , TimeUtil.DATE_FORMAT_yyyyMMddHHmmss) + getChgnameIndex() + 
							".CSV";
		if(chgname.equals(currentChgName))
		{
			try {
				throw new FileNameRepeat("调用CommonUtil.getChgName方法时出现文件名称重复，请注意!!!文件名称:" + chgname);
			} catch (FileNameRepeat e) 
			{
				log.error("调用CommonUtil.getChgName方法时出现文件名称重复，请注意!!!文件名称:" + chgname , e);
				e.printStackTrace();
			}
		}
		currentChgName = chgname;
		return chgname;
	}
	
	public static String getPrefixNameForCsv(String csvName)
	{
		csvName = csvName.toUpperCase();
		return csvName.substring(1 , csvName.indexOf(".CSV"));
	}

	public static String getMsgcode(String msgcode, String storeNo) 
	{
		return msgcode + storeNo;
	}
	
	public static String getMsgcodePrefix(String msgcode , String storeNo)
	{
		if(msgcode.indexOf(storeNo) == -1)
			return msgcode;
		return msgcode.substring(0 , msgcode.indexOf(storeNo));
	}

	public synchronized String getChgnameIndex() 
	{
		if(++ chgnameIndex >= 999)
			chgnameIndex = 0;
		String totle = "" + chgnameIndex;
		if(chgnameIndex < 10)
			totle = "00" + chgnameIndex;
		else if(chgnameIndex < 100)
			totle = "0" + chgnameIndex;
		return totle;
	}
	
}

class Rrr implements Runnable
{
	@Override
	public void run() 
	{
		CommonUtil c = CommonUtil.getInstance();
		System.out.println(c.getChgnameIndex());
	}
}
