package com.hola.bs.print.template;

import java.util.List;
import java.util.Map;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.hola.bs.core.exception.Template6Exception;
import com.hola.bs.print.PrintConfigUtil;

/**
 * 箱明细清单
 * @author roy
 *
 */
public class PrintTemplate6 extends PrintTemplate {
    private static Logger logger = LoggerFactory.getLogger(PrintTemplate6.class);
    
//    private PrintTemplate6() {
//    }
//    public PrintTemplate6(JdbcTemplate jdbcTemplate) {
//        super(jdbcTemplate);
//    }
    @Override
    public String createReport(String path, String storeid, String param[]  , String... detailsqlParams) throws Template6Exception {
    	// 箱明细清单表头
    	String headerSql = PrintConfigUtil.getTemplate6headerSql(storeid);
    	logger.info("箱明细清单表头SQL:{}",headerSql);

    	// 明细SQL
    	String detailSql = PrintConfigUtil.getTemplate6detailSql(storeid);
    	logger.info("箱明细清单明细SQL:{}",detailSql);
    	
    	//调拨收货数量统计SQL
        String totalSql = PrintConfigUtil.getTemplate6TotalSql(storeid);
        logger.info("调拨收货数量统计SQL:{}",totalSql);

    	List<Map<String, Object>> headerMapList1 = jdbcTemplate.queryForList(headerSql, param[0]);
    	List<Map<String, Object>> headerMapList2 = jdbcTemplate.queryForList(totalSql, param[0]);

    	if(headerMapList1.size() == 0){
    		throw new Template6Exception("未取得调入店信息");
    	}

    	Map<String, Object> headerMap1 = headerMapList1.get(0);
    	Map<String, Object> headerMap2 = headerMapList2.get(0);
    	headerMap1.putAll(headerMap2);

    	List<Map<String, Object>> detailMapList = jdbcTemplate.queryForList(detailSql,detailsqlParams);
    	
    	return createReport(path, wb, headerMap1, detailMapList, this.getTemplateName());

    }
    
    @Override
    public String getTemplateName() {
        return PrintConfigUtil.getTemplate6Name();
    }
    
    @Override
    public String getPrinterName() {
        return PrintConfigUtil.getTemplate6PrinterName();
    }
}
