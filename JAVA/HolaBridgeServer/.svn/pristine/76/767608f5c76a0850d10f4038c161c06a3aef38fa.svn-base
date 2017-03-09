package com.hola.bs.print.template;

import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Map.Entry;

import org.apache.poi.ss.usermodel.Sheet;
import org.apache.poi.ss.usermodel.Workbook;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;

import com.hola.bs.print.POIExcelUtil;
import com.hola.bs.print.PrintConfigUtil;
import com.hola.bs.print.Printer;
import com.hola.bs.util.DateUtils;

/**
 * 打印模板
 * @author roy
 *
 */
public abstract class PrintTemplate {
	
	@Autowired(required = true)
    protected JdbcTemplate jdbcTemplate;
    protected Workbook wb;
    
    public PrintTemplate(){
    }
    
    public PrintTemplate(JdbcTemplate jdbcTemplate){
        this.jdbcTemplate = jdbcTemplate;
        // 读取模板
//        wb = POIExcelUtil.readExcel("template/" + getTemplateName());
    }
    
    public static String printByTemplateNo(String templateId, String sqlParams[], String... detailsqlParams){
        try {
            PrintTemplate template = PrintTemplateFactory.getPrintTemplateById(templateId);
            // 根据模板取得打印机，打印模板
            return Printer.print(template, sqlParams,detailsqlParams);
        } catch (Exception e) {
            e.printStackTrace();
            return e.getMessage();
        }
        
    }

    public abstract String createReport(String path, String storeid, String sqlParams[], String... detailsqlParams) throws Exception;
    
    public abstract String getTemplateName();

    public abstract String getPrinterName();
    
    protected String createReport(String path, Workbook wb, Map<String, Object> headerMap, List<Map<String, Object>> detailMapList, String templateName) {
    	if(wb==null){
    		if(getTemplateName().equals(templateName)){
    			wb = POIExcelUtil.readExcel("template/" + getTemplateName());
    		}else{
    			wb = POIExcelUtil.readExcel("template/" + PrintConfigUtil.getTemplate13Name());
    		}
    		
    	}
    		
    	
        Sheet sheet = wb.getSheetAt(0);
        
        Map<String, Object> map = new HashMap<String, Object>();
        for(Entry<String, Object> entry : headerMap.entrySet()){
            map.put("#" + entry.getKey(), entry.getValue());
        }
        
        // 替换Header
        POIExcelUtil.replaceHeader(sheet, map);
        
        // 替换Detail
        POIExcelUtil.replaceDetail(wb, sheet, "#detail", detailMapList);
        
        templateName = System.currentTimeMillis()+templateName;
        
        // 取得临时文件路径
        String tempPath = "\\\\"+path + "\\" + templateName;
        System.out.println("文件路径："+tempPath);
        // 生成报表文件
        POIExcelUtil.createExcel(wb, tempPath);

        return templateName;
    }
    
    
    protected String createReportExecl(String path, Workbook wb, Map<String, Object> headerMap, List<Map<String, Object>> detailMapList, String templateName) {
    	if(wb==null){
    		
    		wb = POIExcelUtil.readExcel("template/" + getTemplateName());
    	}
    		
    	
        Sheet sheet = wb.getSheetAt(0);
        
        Map<String, Object> map = new HashMap<String, Object>();
        for(Entry<String, Object> entry : headerMap.entrySet()){
            map.put("#" + entry.getKey(), entry.getValue());
        }
        
        // 替换Header
        POIExcelUtil.replaceHeader(sheet, map);
        
        // 替换Detail
        POIExcelUtil.replaceDetail(wb, sheet, "#detail", detailMapList);
        
        templateName = DateUtils.format(new Date(), DateUtils.DEFAULT_PAGE_FORMAT)+"_"+headerMap.get("outsto")+".xls";
        
        DateUtils.format(new Date(), DateUtils.DEFAULT_PAGE_FORMAT);
        // 取得临时文件路径
        String tempPath = "\\\\"+path + "\\" + templateName;
        System.out.println("文件路径："+tempPath);
        // 生成报表文件
        POIExcelUtil.createExcel(wb, tempPath);

        return templateName;
    }
    /**
     * 定义一个循环打单的操作
     * @param path
     * @param wb
     * @param headerMapList
     * @param detailMapList
     * @param templateName
     * @return
     * author: S2139
     * 2012 Sep 12, 2012 11:50:53 AM
     */
    protected String[] createReport(String path, Workbook wb, List<Map<String, Object>> headerMapList, List<Map<String, Object>> detailMapList, String templateName) {
    	if(headerMapList!=null&&headerMapList.size()>0){
    		if(headerMapList.size()==1){
    			Map<String, Object> headerMap = headerMapList.get(0);
    			String templatePrintName = createReport(path, wb, headerMap, detailMapList, templateName);
    			return new String[]{templatePrintName};
    		}else{
    			int template_size = headerMapList.size();
    			int no = 0;
    			String[] templatePrintNames = new String[template_size];
    			for(Map<String, Object> headerMap : headerMapList){
    				templatePrintNames[no] = createReport(path, wb, headerMap, detailMapList, templateName);
    				no++; 
    			}
    			return templatePrintNames;
    		}
    	}else{
    		return null;
    	}
    }
    
    public JdbcTemplate getJdbcTemplate() {
        return jdbcTemplate;
    }

    public void setJdbcTemplate(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

    
}
