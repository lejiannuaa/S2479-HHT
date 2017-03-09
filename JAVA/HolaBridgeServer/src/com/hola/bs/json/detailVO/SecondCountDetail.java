package com.hola.bs.json.detailVO;

/**
 * 复盘JSON的明细数据部分
 * @author s2139
 * 2012 Aug 28, 2012 11:05:17 AM 
 */
public class SecondCountDetail {
	
	private int sel_no;
	private String sku;
	private String adj_reason;
	private int sku_second_qty;
	public int getSel_no() {
		return sel_no;
	}
	public void setSel_no(int sel_no) {
		this.sel_no = sel_no;
	}
	public String getSku() {
		return sku;
	}
	public void setSku(String sku) {
		this.sku = sku;
	}
	public String getAdj_reason() {
		return adj_reason;
	}
	public void setAdj_reason(String adj_reason) {
		this.adj_reason = adj_reason;
	}
	public int getSku_second_qty() {
		return sku_second_qty;
	}
	public void setSku_second_qty(int sku_second_qty) {
		this.sku_second_qty = sku_second_qty;
	}
	
	
}
