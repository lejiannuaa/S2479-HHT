package com.hola.bs.property;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.util.Properties;

public class ConfigPropertyUtil {
	Properties props = new Properties();

	public ConfigPropertyUtil() {

		try {
			File f = new File(this.getClass().getResource("/").getFile() + "/config.properties");
//			File f = new File(System.getProperty("user.dir")+"\\config.properties");
			FileInputStream fis = new FileInputStream(f.getPath());
			props.load(fis);
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	
	public String getValue(String name) {
		return props.getProperty(name);
	}

	public int getIntValue(String name){
		return Integer.parseInt(props.getProperty(name));
	}
	public static void main(String[] args) {
		System.out.println((new ConfigPropertyUtil()).getValue("oracle.url"));
	}
}
