package com.hola.bs.print.template;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.hola.bs.print.PrintConfigUtil;

/**
 * PO收货报表
 * @author roy
 *
 */
public class PrintTemplate2 extends PrintTemplate {
    private static Logger logger = LoggerFactory.getLogger(PrintTemplate2.class);
    
//    private PrintTemplate2() {
//    }
//    public PrintTemplate2(JdbcTemplate jdbcTemplate) {
//        super(jdbcTemplate);
//    }

    @Override
    public String createReport(String path, String storeid, String param[]  , String... detailsqlParams) throws Exception {
        // PO收货报表表头SQL
        String headerSql = PrintConfigUtil.getTemplate1headerSql(storeid);
        logger.info("报表表头SQL:{}",headerSql);
        
        // 明细SQL
        String detailSql = PrintConfigUtil.getTemplate2detailSql(storeid);
        logger.info("PO收货报表明细SQL:{}",detailSql);
        
        //统计SQL
        String totalSql = PrintConfigUtil.getTemplate2TotalSql(storeid);
        logger.info("POTotal报表明细SQL:{}",totalSql);
        List<Map<String, Object>> headerMapList = jdbcTemplate.queryForList(headerSql, param);
        
        if(headerMapList.size() == 0){
            throw new Exception("未取得PO收货报表表头信息");
        }
        
        List<Map<String, Object>> totalMapList = jdbcTemplate.queryForList(totalSql, param);
        Map<String, Object> headerMap = headerMapList.get(0);
        Map<String, Object> totalMap = totalMapList.get(0);
        /*
         * 添加单号
         */
        Map<String, Object> map1 = new HashMap<String, Object>();
        map1.put("HHTPO", "*"+param[0]+"*");
        headerMap.putAll(map1);
        headerMap.putAll(totalMap);
        List<Map<String, Object>> detailMapList = jdbcTemplate.queryForList(detailSql,detailsqlParams);
        
        return createReport(path, wb, headerMap, detailMapList, this.getTemplateName());
    }

    @Override
    public String getTemplateName() {
        return PrintConfigUtil.getTemplate2Name();
    }
    @Override
    public String getPrinterName() {
        return PrintConfigUtil.getTemplate2PrinterName();
    }
}
