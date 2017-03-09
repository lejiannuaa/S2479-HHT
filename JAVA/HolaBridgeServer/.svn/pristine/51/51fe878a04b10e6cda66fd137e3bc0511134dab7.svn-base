package com.hola.bs.print.template;

import java.util.List;
import java.util.Map;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;

import com.hola.bs.print.PrintConfigUtil;

/**
 * 出货箱单调拨单的打印
 * @author S2139
 * 2012 Sep 12, 2012 1:16:56 PM 
 */
public class PrintTemplate8Trf extends PrintTemplate {

	private static Logger logger = LoggerFactory.getLogger(PrintTemplate8Trf.class);
	
	public PrintTemplate8Trf(){
		
	}
	
	public PrintTemplate8Trf(JdbcTemplate jdbcTemplate) {
		super(jdbcTemplate);
		// TODO Auto-generated constructor stub
	}
	
	public String createReport(String path, String storeid, String[] sqlParams,
			String... detailsqlParams) throws Exception {
		// TODO Auto-generated method stub
	      // 调拨出货单表头
        String headerSql = PrintConfigUtil.getTemplate8headerTrfSql(storeid);
        logger.info("调拨出货箱单表头SQL:{}",headerSql);
        List<Map<String, Object>> headerMapList = jdbcTemplate.queryForList(headerSql, sqlParams[0],sqlParams[1],sqlParams[2]);
        
        if(headerMapList.size() == 0){
            throw new Exception("未取得调拨箱单表头信息");
        }
        Map<String, Object> headerMap = headerMapList.get(0);
//      return createReport(path, wb, headerMapList, null, this.getTemplateName());
        return createReport(path, wb, headerMap, null, this.getTemplateName());
	}
	
	public String[] createTrfReport(String path, String storeid, String[] sqlParams,
			String... detailsqlParams) throws Exception{
			// TODO Auto-generated method stub
		      // 调拨出货单表头
	        String headerSql = PrintConfigUtil.getTemplate8headerTrfSql(storeid);
	        logger.info("调拨出货箱单表头SQL:{}",headerSql);
	        List<Map<String, Object>> headerMapList = jdbcTemplate.queryForList(headerSql, sqlParams[0],sqlParams[1],sqlParams[2]);
	        
	        if(headerMapList.size() == 0){
	            throw new Exception("未取得调拨箱单表头信息");
	        }
	        
	        return createReport(path, wb, headerMapList, null, this.getTemplateName());

	}
	
	public String getPrinterName() {
        return PrintConfigUtil.getTemplate8PrinterName();
    }

    public String getTemplateName() {
        return PrintConfigUtil.getTemplate8Name();
    }

}
