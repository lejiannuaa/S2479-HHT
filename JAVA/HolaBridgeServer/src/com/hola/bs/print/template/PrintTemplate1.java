package com.hola.bs.print.template;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.hola.bs.print.PrintConfigUtil;

/**
 * PO差异报表
 * 
 * @author roy
 * 
 */
public class PrintTemplate1 extends PrintTemplate {
    private static Logger logger = LoggerFactory.getLogger(PrintTemplate1.class);
    
    
//    private PrintTemplate1() {
//    }
//    public PrintTemplate1(JdbcTemplate jdbcTemplate) {
//        super(jdbcTemplate);
//    }

    @Override
    public String createReport(String path, String storeid, String param[]  , String... detailsqlParams) throws Exception {
        // 
        String headerSql = PrintConfigUtil.getTemplate1headerSql(storeid);
        logger.info("PO差异表头SQL:{}",headerSql);
        
        // 明细SQL
        String detailSql = PrintConfigUtil.getTemplate1detailSql(storeid);
        logger.info("PO差异明细SQL:{}",detailSql);
        
        String totalDifSql = PrintConfigUtil.getTemplate1TotalDifSql(storeid);
        logger.info("PO差异总计SQL:{}",totalDifSql);

        List<Map<String, Object>> headerMapList = jdbcTemplate.queryForList(headerSql, param);
        List<Map<String, Object>> totalDifMapList = jdbcTemplate.queryForList(totalDifSql, param);
        if(headerMapList.size() == 0){
            throw new Exception("查无该单号");
        }
        
        Map<String, Object> headerMap = headerMapList.get(0);
        Map<String, Object> totalMap = totalDifMapList.get(0);
        /*
         * 添加单号
         */
        Map<String, Object> map1 = new HashMap<String, Object>();
        map1.put("HHTNO", "*"+param[0]+"*");
        headerMap.putAll(map1);
        headerMap.putAll(totalMap);
        List<Map<String, Object>> detailMapList = jdbcTemplate.queryForList(detailSql,detailsqlParams);
        for(Map m : detailMapList){
        	if(m.get("D6LCRQ")==null){
        		m.put("D6LCRQ", "");
        	}
        }
        
        return createReport(path, wb, headerMap, detailMapList, this.getTemplateName());
        
    }

    @Override
    public String getTemplateName() {
        return PrintConfigUtil.getTemplate1Name();
    }


    @Override
    public String getPrinterName() {
        return PrintConfigUtil.getTemplate1PrinterName();
    }


    public static void main(String[] args){
    	String sql = "SELECT sum(D6LCRQ) as TTDIF from schema.JDAOD6F where D6CNO = ? ";
    	System.out.println(sql.replaceAll("schema", "13101"));
    }
    
    
}
