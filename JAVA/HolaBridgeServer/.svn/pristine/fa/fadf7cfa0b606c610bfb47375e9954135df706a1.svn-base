package com.hola.bs.print.template;

import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;

import com.hola.bs.print.PrintConfigUtil;
import com.hola.bs.util.DateUtils;

public class PrintTemplate9 extends PrintTemplate {
	 
	private static Logger logger = LoggerFactory.getLogger(PrintTemplate9.class);
	
	public PrintTemplate9() {
	}
	
	public PrintTemplate9(JdbcTemplate jdbcTemplate) {
	        super(jdbcTemplate);
	}

	@Override
	public String createReport(String path, String storeid, String[] sqlParams,
			String... detailsqlParams) throws Exception {
		// TODO Auto-generated method stub

		String detailSql = PrintConfigUtil.getTemplate9detailSql(storeid);

		Map<String, Object> headerMap = new HashMap<String, Object>();
		headerMap.put("applyday", DateUtils.date2String(new Date()));
		headerMap.put("appleperson", sqlParams[2].toString());
		List<Map<String, Object>> detailMapList = jdbcTemplate.queryForList(detailSql,sqlParams);
		return createReport(path, wb, headerMap, detailMapList, this.getTemplateName());
	}
	
	public String[] createReportList(String path, String storeid, String[] sqlParams,
			String... detailsqlParams) throws Exception {
		
		String detailSql = PrintConfigUtil.getTemplate9detailSql(storeid);

		Map<String, Object> headerMap = new HashMap<String, Object>();
		headerMap.put("applyday", DateUtils.date2String(new Date()));
		headerMap.put("appleperson", sqlParams[2].toString());
		List<Map<String, Object>> detailMapList = jdbcTemplate.queryForList(detailSql,sqlParams);
		
		String[] list = new String[detailMapList.size()/30+1];
		
		for(int i=0;i<list.length;i++){
			List<Map<String, Object>> tDetailMapList = new ArrayList<Map<String,Object>>();
			for(int j=0;j<30;j++){
				if (i!=0&&(30*i+j)<detailMapList.size()){
					
					tDetailMapList.add(detailMapList.get(30*i+j));
				}else if(i==0 && j< detailMapList.size()){
					
					tDetailMapList.add(detailMapList.get(j));
				}
			}
			
			list[i]=createReport(path, wb, headerMap, tDetailMapList, this.getTemplateName());
			Thread.sleep(50);
		}
		return list;
		
		
	}
	
	public String[] createReportListSubmit(String path, String storeid, String[] sqlParams,List<Map<String, Object>> detailMapList
			) throws Exception {
		
		String detailSql = PrintConfigUtil.getTemplate9detailSql(storeid);

		Map<String, Object> headerMap = new HashMap<String, Object>();
		headerMap.put("applyday", DateUtils.date2String(new Date()));
		headerMap.put("appleperson", sqlParams[2].toString());
		
		
		String[] list = new String[detailMapList.size()/30+1];
		
		for(int i=0;i<list.length;i++){
			List<Map<String, Object>> tDetailMapList = new ArrayList<Map<String,Object>>();
			for(int j=0;j<30;j++){
				if (i!=0&&(30*i+j)<detailMapList.size()){
					
					tDetailMapList.add(detailMapList.get(30*i+j));
				}else if(i==0 && j< detailMapList.size()){
					
					tDetailMapList.add(detailMapList.get(j));
				}
			}
			
			list[i]=createReport(path, wb, headerMap, tDetailMapList, this.getTemplateName());
			Thread.sleep(50);
		}
		return list;
		
		
	}

	@Override
	public String getTemplateName() {
		// TODO Auto-generated method stub
		return PrintConfigUtil.getTemplate9Name();
	}

	@Override
	public String getPrinterName() {
		// TODO Auto-generated method stub
		return PrintConfigUtil.getTemplate9PrinterName();
	}

}
