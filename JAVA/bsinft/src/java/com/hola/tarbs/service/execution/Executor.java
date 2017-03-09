package com.hola.tarbs.service.execution;

import java.util.Date;
import java.util.List;

import com.hola.common.constant.DefvalForChgtbldtl;
import com.hola.common.model.mw.ChgTbl;
import com.hola.common.model.mw.ChgTblDtl;
import com.hola.common.util.StringUtil;
import com.hola.common.util.TimeUtil;

public abstract class Executor 
{
	public static final String TAR_OP_TYP_INSERT = "0";
	public static final String TAR_OP_TYP_DEL_INSERT = "1";
	
	/**
	 * 构建sql
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-5 上午11:27:20
	 * @param data
	 * @param config
	 * @param schema
	 * @param instno 
	 * @return
	 */
	public abstract List<String> sqlCreate(Object data , Object config , String schema, String instno);
	
	/**
	 * 工厂方法
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-4 上午11:18:42
	 * @param taroptyp
	 * @return
	 */
	public static Executor getExcutor(String taroptyp)
	{
		taroptyp = taroptyp.trim();
		if(Executor.TAR_OP_TYP_DEL_INSERT.equals(taroptyp))
		{
			return new Del_InsertForBs();
		} else if(Executor.TAR_OP_TYP_INSERT.equals(taroptyp))
		{
			return new InsertForBs();
		}
		return null;
	}
	
	public void createInsert(String schema, String[] datas, ChgTbl chgtbl,
			StringBuffer sql, String instno) 
	{
		sql.append(" insert into ").append(schema).append(".").append(chgtbl.getTarTableName())
						.append(" (").append(chgtbl.getColStr()).append(") values (");
		//赋值bs的instno
		datas[chgtbl.getInstnoColNo() - 1] = instno;
		//拼值
		List<ChgTblDtl> chgtblDtlList = chgtbl.getCols();
//		int i = 0;
//		int index = datas.length;
		for (int i = 0 ; i < chgtblDtlList.size() ; i ++)
		{
			ChgTblDtl chgTblDtl = chgtblDtlList.get(i);
			
			if(i > 0)
				sql.append(" , ");
			String value = ""; 
			
			if(i < datas.length)	//JDA栏位有BS栏位也有，进入此块
				value = datas[i];
			else					//JDA没有栏位，BS有此栏位，进入此块，赋默认值
			{
				value = chgTblDtl.getDefaultValue();
				if(value == null)
					value = "";
				if(value.equals(DefvalForChgtbldtl.SYSDATE))	//如果默认值是系统时间，赋值系统时间:yyyyMMdd
					value = TimeUtil.formatDate(new Date(), TimeUtil.DATE_FORMAT_yyyyMMdd);
			}
				
			
			if(value.equals("") || value == null)
			{
				sql.append("null");
			} else
			{
				value = StringUtil.replaceAll(value, "'", "''");
				//value = StringUtil.replaceAll(value, "'", "\\'");
				sql.append("'").append(value).append("'");
			}
		}
//		for (int i = 0; i < datas.length; i++) 
//		{
//			
//			
//		}
		sql.append(")");
	}

	public void createDel(String [] datas, ChgTbl chgtbl, String schema,
			StringBuffer sql) 
	{
		
		sql.append("delete from ").append(schema).append(".").append(chgtbl.getTarTableName())
					.append(" where ");
		String [] pkCols = chgtbl.getPkColStr().split(",");
		for (int i = 0; i < pkCols.length; i++) 
		{
			if(i > 0)
				sql.append(" and ");
			int pkIndex = chgtbl.getPkColNo().get(i) - 1;
			String currentPkVal = datas[pkIndex];
			if(currentPkVal.equals(""))
			{//(HHTNO='' or  HHTNO is null)
				sql.append("(").append(pkCols[i]).append("='' or ").append(pkCols[i]).append(" is null)");
			} else
			{
				sql.append(pkCols[i]).append("='").append(datas[pkIndex].trim()).append("'");
			}
		}
	}
}
