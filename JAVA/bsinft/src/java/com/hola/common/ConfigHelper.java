package com.hola.common;

import java.io.IOException;
import java.util.Date;
import java.util.Properties;

import com.hola.common.util.TimeUtil;

/**
 * 璇诲彇閰嶇疆config.properties
 * @author 鐜嬫垚(chengwangi@isoftstone.com)
 * @date 2013-1-4 涓嬪崍2:02:16
 */
public class ConfigHelper 
{
	public static final String CSV_FILE_DIRECTORY			=		"csv_file_directory";
	public static final String FOLDER_NAME_FOR_MQ_CSV_BS	=		"folder_name_for_from_mq_to_bs";
	public static final String FOLDER_NAME_FOR_BS_CSV_MQ	=		"folder_name_for_from_bs_to_mq";
	public static final String FOLDER_NAME_FOR_ROOT			=		"folder_name_for_root";
	public static final String FOLDER_NAME_FOR_DISK			=		"folder_name_for_disk";
	
	
	
	public static final String THREADPOOL_QUEUE_SIZE				=		"thread_queue_size";
	public static final String THREADPOOL_CORE_POOL_SIZE			=		"corePoolSize";
	public static final String THREADPOOL_MAX_POOL_SIZE				=		"maxPoolSize";
	public static final String THREADPOOL_KEEP_ALVIE_TIME			=		"keepAlvieTime";
	public static final String THREADPOOL_MAX_POOL_SIZE_RECEIV_MQ	=		"maxPoolSizeForReceivFileMq";
		
	
	public static ConfigHelper ch;
	private	final	static	Properties	properties = init();
	
	private ConfigHelper(){}
	
	public synchronized static ConfigHelper getInstance()
	{
		if(ch == null)
			ch = new ConfigHelper();
		return ch;
	}
	private static Properties init() 
	{
		Properties	properties = new Properties(); 
		try {
			properties.load(ConfigHelper.class.getClassLoader().getResourceAsStream("config.properties"));
			return properties;
		} catch (IOException e) {
			e.printStackTrace();
		}
		return null;
	}
	
	public String getValue(String key)
	{
		return (String) properties.get(key);
	}
	
	public int getIntValue(String key)
	{
		return Integer.parseInt((String) properties.get(key));
	}
	
	public Date getDateValue(String key , String dateFormat)
	{
		String str = (String) properties.get(key);
		return TimeUtil.parseDay(str, dateFormat);
	}
}