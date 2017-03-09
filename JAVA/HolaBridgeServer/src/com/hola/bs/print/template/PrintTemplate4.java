package com.hola.bs.print.template;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.hola.bs.print.PrintConfigUtil;

/**
 * 调拨收货
 * @author roy
 *
 */
public class PrintTemplate4 extends PrintTemplate {
    private static Logger logger = LoggerFactory.getLogger(PrintTemplate4.class);
    
//    private PrintTemplate4() {
//    }
//    public PrintTemplate4(JdbcTemplate jdbcTemplate) {
//        super(jdbcTemplate);
//    }
    @Override
    public String createReport(String path, String storeid, String param[]  , String... detailsqlParams) throws Exception {
        // 调拨收货调入店
        String headerSql1 = PrintConfigUtil.getTemplate3headerSql1(storeid);
        logger.info("调拨收货调入店SQL:{}",headerSql1);
        
        // 调拨收货调出店
        String headerSql2 = PrintConfigUtil.getTemplate3headerSql2(storeid);
        logger.info("调拨收货调出店SQL:{}",headerSql2);
        
        // 调拨收货明细SQL
        String detailSql = PrintConfigUtil.getTemplate4detailSql(storeid);
        logger.info("调拨收货明细SQL:{}",detailSql);
        
        //调拨收货数量统计SQL
        String totalSql = PrintConfigUtil.getTemplate4TotalSql(storeid);
        logger.info("调拨收货数量统计SQL:{}",totalSql);
        
        List<Map<String, Object>> headerMapList1 = jdbcTemplate.queryForList(headerSql1, param[0]);
        List<Map<String, Object>> headerMapList2 = jdbcTemplate.queryForList(headerSql2, param[0]);
        List<Map<String, Object>> headerMapList3 = jdbcTemplate.queryForList(totalSql, param[0]);
        
        if(headerMapList1.size() == 0){
            throw new Exception("未取得调拨收货调入店信息");
        }
        if(headerMapList2.size() == 0){
            throw new Exception("未取得调拨收货调出店信息");
        }
        
        /*
         * 添加箱号，增加前缀和后缀*，用来转换成条码
         */
        Map<String, Object> map3 = new HashMap<String, Object>();
        map3.put("HHTCNO", "*"+param[0]+"*");
        
        Map<String, Object> headerMap = headerMapList1.get(0);
        Map<String, Object> headerMap1 = headerMapList2.get(0);
        Map<String, Object> headerMap2 = headerMapList3.get(0);
        
        headerMap.putAll(headerMap1);
        headerMap.putAll(headerMap2);
        headerMap.putAll(map3);

        List<Map<String, Object>> detailMapList = jdbcTemplate.queryForList(detailSql,detailsqlParams);
        
        
        return createReport(path, wb, headerMap, detailMapList, this.getTemplateName());
    }

    @Override
    public String getTemplateName() {
        return PrintConfigUtil.getTemplate4Name();
    }
    @Override
    public String getPrinterName() {
        return PrintConfigUtil.getTemplate4PrinterName();
    }
}
