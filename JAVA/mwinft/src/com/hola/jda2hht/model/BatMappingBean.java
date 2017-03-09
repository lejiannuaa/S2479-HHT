package com.hola.jda2hht.model;

import java.io.Serializable;

/**
 * 文件分批后的映射文件
 * 
 * @author 唐植超(上海软通)
 * @date 2013-1-7
 */
public class BatMappingBean implements Serializable {

	private static final long serialVersionUID = 1L;

	// 交换代码
	private String chgcode;
	// 交换批次号
	private String instno;
	// 文件名
	private String filname;
	// 建立时间
	private String crttime;

	public String getChgcode() {
		return chgcode;
	}

	public void setChgcode(String chgcode) {
		this.chgcode = chgcode;
	}

	public String getInstno() {
		return instno;
	}

	public void setInstno(String instno) {
		this.instno = instno;
	}

	public String getFilname() {
		return filname;
	}

	public void setFilname(String filname) {
		this.filname = filname;
	}

	public String getCrttime() {
		return crttime;
	}

	public void setCrttime(String crttime) {
		this.crttime = crttime;
	}
}
