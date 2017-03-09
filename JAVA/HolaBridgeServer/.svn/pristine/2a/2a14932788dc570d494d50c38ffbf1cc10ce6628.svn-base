package com.hola.bs.print.template;

import java.util.HashMap;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;

public class PrintTemplateFactory {
	
	@Autowired(required = true)
    private JdbcTemplate jdbcTemplate;
	
    private static Map<String,PrintTemplate> printTemplateMap = new HashMap<String,PrintTemplate>();
    
    public static PrintTemplate getPrintTemplateById(String id){
        return printTemplateMap.get(PrintTemplate.class.getName() + id);
    }
    
    public JdbcTemplate getJdbcTemplate() {
        return jdbcTemplate;
    }

    public void setJdbcTemplate(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

	public Map<String, PrintTemplate> getPrintTemplateMap() {
		return printTemplateMap;
	}

	public void setPrintTemplateMap(Map<String, PrintTemplate> printTemplateMap) {
		PrintTemplateFactory.printTemplateMap = printTemplateMap;
	}
    
    

}
