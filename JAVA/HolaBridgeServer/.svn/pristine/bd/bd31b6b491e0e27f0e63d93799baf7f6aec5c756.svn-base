package com.hola.bs.print.template;

import java.util.List;
import java.util.Map;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.hola.bs.print.PrintConfigUtil;

/**
 * 调拨差异报表
 * @author roy
 *
 */
public class PrintTemplate3 extends PrintTemplate {
    private static Logger logger = LoggerFactory.getLogger(PrintTemplate3.class);
    
    @Override
    public String createReport(String path, String storeid, String param[]  , String... detailsqlParams) throws Exception {
        // 调入店
        String headerSql1 = PrintConfigUtil.getTemplate3headerSql1(storeid);
        logger.info("调拨差异报表调入店SQL:{}",headerSql1);
        
        // 调出店
        String headerSql2 = PrintConfigUtil.getTemplate3headerSql2(storeid);
        logger.info("调拨差异报表调出店SQL:{}",headerSql2);
        
        // 明细SQL
        String detailSql = PrintConfigUtil.getTemplate3detailSql(storeid);
        logger.info("调拨差异报表明细SQL:{}",detailSql);
        
        //差异总计
        String difTotalSql = PrintConfigUtil.getTemplate3DifTotalSql(storeid);
        logger.info("调拨差异明细总计SQL:{}",difTotalSql);
        
        List<Map<String, Object>> headerMapList1 = jdbcTemplate.queryForList(headerSql1, param[0]);
        List<Map<String, Object>> headerMapList2 = jdbcTemplate.queryForList(headerSql2, param[0]);
        List<Map<String, Object>> headerMapList3 = jdbcTemplate.queryForList(difTotalSql, param[0]);
        if(headerMapList1.size() == 0){
            throw new Exception("未取得调拨差异报表调入店信息");
        }
        if(headerMapList2.size() == 0){
            throw new Exception("未取得调拨差异报表调出店信息");
        }
        
        Map<String, Object> headerMap = headerMapList1.get(0);
        Map<String, Object> headerMap1 = headerMapList2.get(0);
        Map<String, Object> headerMap3 = headerMapList3.get(0);
        headerMap.putAll(headerMap1);
        headerMap.put("HHTCNO", "*"+param[0]+"*");
        headerMap.putAll(headerMap3);
        List<Map<String, Object>> detailMapList = jdbcTemplate.queryForList(detailSql,detailsqlParams);
        
        return createReport(path, wb, headerMap, detailMapList, this.getTemplateName());
    }

    @Override
    public String getTemplateName() {
        return PrintConfigUtil.getTemplate3Name();
    }
    @Override
    public String getPrinterName() {
        return PrintConfigUtil.getTemplate3PrinterName();
    }
}
