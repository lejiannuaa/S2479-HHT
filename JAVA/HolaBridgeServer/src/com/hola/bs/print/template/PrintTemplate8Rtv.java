package com.hola.bs.print.template;

import java.util.List;
import java.util.Map;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;

import com.hola.bs.print.PrintConfigUtil;

/**
 * 特力实物退货出货箱单打印
 * @author S2139
 * 2012 Sep 11, 2012 11:28:31 AM 
 */
public class PrintTemplate8Rtv extends PrintTemplate {
	private static Logger logger = LoggerFactory.getLogger(PrintTemplate8Rtv.class);
	
	public PrintTemplate8Rtv(){
		
	}
	
	public PrintTemplate8Rtv(JdbcTemplate jdbcTemplate) {
		super(jdbcTemplate);
		// TODO Auto-generated constructor stub
	}
	
	public String[] createRtvReport(String path, String storeid, String[] sqlParams,
			String... detailsqlParams) throws Exception {
		// TODO Auto-generated method stub
	      // 调拨出货单表头
        String headerSql = PrintConfigUtil.getTemplate8headerRtvSql(storeid);
        logger.info("退货出货箱单表头SQL:{}",headerSql);
        System.out.println("SQL参数依次为："+sqlParams[0]+", "+sqlParams[1]+", "+sqlParams[2]);
        List<Map<String, Object>> headerMapList = jdbcTemplate.queryForList(headerSql, sqlParams[0],sqlParams[1],sqlParams[2]);
        if(headerMapList.size() == 0){
            throw new Exception("未取得退货箱单表头信息");
        }
        
        return createReport(path, wb, headerMapList, null, this.getTemplateName());

	}

	public String getPrinterName() {
        return PrintConfigUtil.getTemplate8PrinterName();
    }

    public String getTemplateName() {
        return PrintConfigUtil.getTemplate8Name();
    }

	public String createReport(String path, String storeid, String[] sqlParams,
			String... detailsqlParams) throws Exception {
		// TODO Auto-generated method stub
        String headerSql = PrintConfigUtil.getTemplate8headerRtvSql(storeid);
        logger.info("退货出货箱单表头SQL:{}",headerSql);
        System.out.println("SQL参数依次为："+sqlParams[0]+", "+sqlParams[1]+", "+sqlParams[2]);
        List<Map<String, Object>> headerMapList = jdbcTemplate.queryForList(headerSql, sqlParams[0],sqlParams[1],sqlParams[2]);
        
        if(headerMapList.size() == 0){
            throw new Exception("未取得退货箱单表头信息");
        }
        
        Map<String, Object> headerMap = headerMapList.get(0);
//        return createReport(path, wb, headerMapList, null, this.getTemplateName());
        return createReport(path, wb, headerMap, null, this.getTemplateName());
	}

}
