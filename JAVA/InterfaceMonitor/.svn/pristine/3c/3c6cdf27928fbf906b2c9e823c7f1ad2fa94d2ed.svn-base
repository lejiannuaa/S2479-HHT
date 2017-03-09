package common;

import java.io.IOException;
import java.util.Properties;


public class ConfigHelper 
{
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
	
}