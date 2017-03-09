package com.hola.bs.print;

import org.apache.commons.configuration.Configuration;
import org.apache.commons.configuration.ConfigurationException;
import org.apache.commons.configuration.PropertiesConfiguration;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;


public class ConfigUtils {
    protected Log logger = LogFactory.getLog(getClass());
    
	private final String ENCODING = "UTF-8";

	private PropertiesConfiguration configuration ;

	private  PropertiesConfiguration initialize(String fileName) {
		PropertiesConfiguration config = null;
		config = new PropertiesConfiguration();
		config.setEncoding(ENCODING);
		try {
			config.load(fileName);
		} catch (ConfigurationException e) {
		    logger.error(e);
		}
		return config;
	}

	public ConfigUtils(String fileName) {
		configuration = initialize(fileName);
	}

	public Configuration geConfig() {
		return configuration;
	}
}
