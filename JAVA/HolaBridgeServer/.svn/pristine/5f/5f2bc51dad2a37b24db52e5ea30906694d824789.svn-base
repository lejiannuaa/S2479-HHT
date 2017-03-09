package com.hola.bs.print.template;

import java.util.List;
import java.util.Map;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.hola.bs.print.PrintConfigUtil;

/**
 * 退货单
 * @author roy
 *
 */
public class PrintTemplate5 extends PrintTemplate {
    private static Logger logger = LoggerFactory.getLogger(PrintTemplate5.class);
    
//    private PrintTemplate5() {
//    }
//    public PrintTemplate5(JdbcTemplate jdbcTemplate) {
//        super(jdbcTemplate);
//    }
    @Override
    public String createReport(String path, String storeid, String param[]  , String... detailsqlParams) throws Exception {
        // 退货单表头
        String headerSql = PrintConfigUtil.getTemplate5headerSql(storeid);
        logger.info("退货单表头SQL:{}",headerSql);
        // 退货单明细SQL
        String detailSql = PrintConfigUtil.getTemplate5detailSql(storeid);
        logger.info("退货单明细SQL:{}",detailSql);
        
        String totalSql = PrintConfigUtil.getTemplate5TotalSql(storeid);
        logger.info("退货单总计SQL:{}",totalSql);
        List<Map<String, Object>> headerMapList = jdbcTemplate.queryForList(headerSql, param[0], param[1]);
        List<Map<String, Object>> headerMapList2 = jdbcTemplate.queryForList(totalSql, param[0], param[1]);
        
        if(headerMapList.size() == 0){
            throw new Exception("未取得退货单表头信息");
        }
        
        Map<String, Object> headerMap = headerMapList.get(0);
        Map<String, Object> headerMap1 = headerMapList2.get(0);
        
        headerMap.putAll(headerMap1);
        
        List<Map<String, Object>> detailMapList = jdbcTemplate.queryForList(detailSql,detailsqlParams);
        if(param[2].equals("1")){
        	 return createReport(path, wb, headerMap, detailMapList, this.getTemplateName5Z());
        }else{
             return createReport(path, wb, headerMap, detailMapList, this.getTemplateName());
        }
    }
    
    @Override
    public String getTemplateName() {
        return PrintConfigUtil.getTemplate5Name();
    }
    
    @Override
    public String getPrinterName() {
        return PrintConfigUtil.getTemplate5PrinterName();
    }
    public String getTemplateName5Z(){
    	return PrintConfigUtil.getTemplate13Name();
    }
}
