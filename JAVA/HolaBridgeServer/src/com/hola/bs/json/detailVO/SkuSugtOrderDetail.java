package com.hola.bs.json.detailVO;

/**
 * 商品建议下单数据明细
 * @author S2139
 * 2012 Aug 30, 2012 2:06:09 PM 
 */
public class SkuSugtOrderDetail {
	
	private String sku;
	private String sku_dsc;
	private String plu_mango;
	private int sku_order_qty;
	public String getSku() {
		return sku;
	}
	public void setSku(String sku) {
		this.sku = sku;
	}
	public String getSku_dsc() {
		return sku_dsc;
	}
	public void setSku_dsc(String sku_dsc) {
		this.sku_dsc = sku_dsc;
	}
	public String getPlu_mango() {
		return plu_mango;
	}
	public void setPlu_mango(String plu_mango) {
		this.plu_mango = plu_mango;
	}
	public int getSku_order_qty() {
		return sku_order_qty;
	}
	public void setSku_order_qty(int sku_order_qty) {
		this.sku_order_qty = sku_order_qty;
	}
	
	
}
