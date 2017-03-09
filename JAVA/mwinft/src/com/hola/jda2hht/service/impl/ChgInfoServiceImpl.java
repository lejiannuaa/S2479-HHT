package com.hola.jda2hht.service.impl;

import java.util.List;

import com.hola.jda2hht.dao.BaseDao;
import com.hola.jda2hht.model.ChangeInfoBean;
import com.hola.jda2hht.model.ChangeSysBean;
import com.hola.jda2hht.service.IChgInfoService;

public class ChgInfoServiceImpl implements IChgInfoService {

	private BaseDao dao;

	public BaseDao getDao() {
		return dao;
	}

	public void setDao(BaseDao dao) {
		this.dao = dao;
	}

	@Override
	@SuppressWarnings("unchecked")
	public List<ChangeInfoBean> getChageBySys(ChangeSysBean sysBean) {
		String sql = "select SYSCODE,CHGCODE,CHGNAME,CHGFQ,CHGFU,STARTTIME,"
				+ "IOTYPE,CHGTYPE,CHGMED,SRCCODE,MSGCODE,MSGSEQTYP,SEQCODE,"
				+ "SEQLEG,MSGFORMAT,CURSTATUS,STATUS,MQIP,MQUSERNAME,MQPWD,"
				+ "QMGNAME,QMGPORT,OQNAME,IQNAME,QCCSID,FTPURL,FTPUSERNAME,"
				+ "FTPPWD,TARDBURL,TARDBDRIVER,TARDBSCHEMA,TARDBUSERNAME,TARDBPWD,"
				+ "REMARK,CREATETIME,ISALLSTR,ISBAT,BATCNT,ISMERGE "
				+ "from HOLA_APP_CFG_CHGINFO where SYSCODE=? and STATUS='E'";
		return (List<ChangeInfoBean>) this.dao.getList(sql,
				new Object[] { sysBean.getSyscode() }, ChangeInfoBean.class);
	}
}
