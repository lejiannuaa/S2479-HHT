package com.hola.jda2hht.service.impl;

import java.util.List;

import org.apache.log4j.Logger;

import com.hola.jda2hht.dao.BaseDao;
import com.hola.jda2hht.model.ChangeSysBean;
import com.hola.jda2hht.service.IChgSysService;

/**
 * 系统表的业务实现
 * 
 * @author 唐植超(上海软通)
 * @date 2012-12-27
 */
public class ChgSysServiceImpl implements IChgSysService {

	private static final Logger log = Logger.getLogger(ChgSysServiceImpl.class);
	private BaseDao dao;

	public BaseDao getDao() {
		return dao;
	}

	public void setDao(BaseDao dao) {
		this.dao = dao;
	}

	@SuppressWarnings("unchecked")
	public List<ChangeSysBean> findAllSys() {
		log.debug("查询所有的系统 findAllSys");
		String sql = "select SYSCODE,SYSNAME,DBURL,DBDRIVER,DBSCHEMA,USERNAME,PASSWORD,DBTYPE,STATUS,TYPE,REMARK,CREATETIME from HOLA_APP_CFG_CHGSYS where STATUS='E'";
		return (List<ChangeSysBean>) this.dao.getList(sql, null,
				ChangeSysBean.class);
	}

}
