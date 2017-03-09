package com.hola.bs.print.template;

import java.util.List;
import java.util.Map;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;

import com.hola.bs.print.PrintConfigUtil;

/**
 * 调拨出货单
 * @author roy
 *
 */
public class PrintTemplate7 extends PrintTemplate {
    private static Logger logger = LoggerFactory.getLogger(PrintTemplate7.class);
    
    public PrintTemplate7() {
    }
    public PrintTemplate7(JdbcTemplate jdbcTemplate) {
        super(jdbcTemplate);
    }
    @Override
    public String createReport(String path, String stroeid, String param[]  , String... detailsqlParams) throws Exception {
        // 调拨出货单表头
        String headerSql = PrintConfigUtil.getTemplate7headerSql(stroeid);
        logger.info("调拨出货单表头SQL:{}",headerSql);
        
        // 明细SQL
        String detailSql = PrintConfigUtil.getTemplate7detailSql(stroeid);
        logger.info("调拨出货单明细SQL:{}",detailSql);
        
        List<Map<String, Object>> headerMapList = jdbcTemplate.queryForList(headerSql, param[0],param[1]);
        
        if(headerMapList.size() == 0){
            throw new Exception("未取得调拨出货单表头信息");
        }
        
        Map<String, Object> headerMap = headerMapList.get(0);
        
        
        List<Map<String, Object>> detailMapList = jdbcTemplate.queryForList(detailSql,detailsqlParams);
        
        return createReport(path, wb, headerMap, detailMapList, this.getTemplateName());
    }
    
    @Override
    public String getTemplateName() {
        return PrintConfigUtil.getTemplate7Name();
    }
    
    @Override
    public String getPrinterName() {
        return PrintConfigUtil.getTemplate7PrinterName();
    }
}
