package com.hola.common.model.mw;

import java.util.Date;
import com.hola.common.model.BaseModel;
import com.hola.common.util.TimeUtil;

public class BsLog extends BaseModel 
{
	public static final String STATUS_SUCESS 	= "S";
	public static final String STATUS_FAILURE	= "F";
	
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	private		String		id;
	private		String		instno;
	private		String		filName;
	private		String		status;//成功:S , 失败:F
	private		String		remark;
	private		String		crtTime;;
	private		String		storeNo;
	
	public String getId() {
		return id;
	}
	public void setId(String id) {
		this.id = id;
	}
	public String getInstno() {
		return instno;
	}
	public void setInstno(String instno) {
		this.instno = instno;
	}
	public String getFilName() {
		return filName;
	}
	public void setFilName(String filName) {
		this.filName = filName;
	}
	public String getStatus() {
		return status;
	}
	public void setStatus(String status) {
		this.status = status;
	}
	public String getRemark() {
		return remark;
	}
	public void setRemark(String remark) {
		this.remark = remark;
	}
	public String getId(String storeNo)
	{
		return storeNo + TimeUtil.formatDate(new Date(), TimeUtil.DATE_FORMAT_yyyyMMddHHmmssSSS);
	}
	public String getCrtTime() {
		return crtTime;
	}
	public void setCrtTime(String crtTime) {
		this.crtTime = crtTime;
	}
	public String getStoreNo() {
		return storeNo;
	}
	public void setStoreNo(String storeNo) {
		this.storeNo = storeNo;
	}
	
}
