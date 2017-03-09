package com.hola.tarbs.service.execution;

import java.util.ArrayList;
import java.util.List;
import com.hola.common.model.mw.ChgTbl;
/**
 * 先delete后insert的执行器
 * @author 王成(chengwangi@isoftstone.com)
 * @date 2013-1-5 上午11:27:47
 */
class Del_InsertForBs extends Executor 
{
	
	@Override
	public List<String> sqlCreate(Object data , Object config , String schema , String instno) 
	{
		List<String> sqls = new ArrayList<String>();
		String [] datas = (String[]) data;
		ChgTbl chgtbl = (ChgTbl) config;
		StringBuffer sql = new StringBuffer(2);
		
		createDel(datas, chgtbl, schema, sql);
		sqls.add(sql.toString());
		sql = new StringBuffer();
//		sql.append(";");
		createInsert(schema, datas, chgtbl, sql , instno);
		sqls.add(sql.toString());
		return sqls;
	}
	
}
