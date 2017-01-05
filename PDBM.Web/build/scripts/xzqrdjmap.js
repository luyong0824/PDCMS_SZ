webpackJsonp([19], {
	0: function(e, n, i) {
		i(1), e.exports = i(353)
	},
	353: function(e, n, i) {
		"use strict";

		function t(e) {
			return e && e.__esModule ? e : {
			default:
				e
			}
		}
		function o() {
			d.clearOverlays();
			var e = new BMap.Point(p, w),
				n = new BMap.Marker(e),
				i = null;
			i = new BMap.Label("测试测试", {
				offset: new BMap.Size(-10, -32)
			}), i.setStyle({
				borderColor: "#ccc",
				fontSize: "14px",
				color: "#333",
				borderRadius: "3px",
				padding: "6px 4px"
			}), n.addEventListener("click", function() {
				window.location.href = window.location.href.split("views")[0] + "views/circumdetail.html"
			}), n.setLabel(i), d.addOverlay(n), d.panTo(e)
		}
		i(299), i(301), i(302);
		var a = i(305),
			r = (t(a), i(312)),
			l = t(r),
			c = i(310),
			p = c.helpers.getSearchString("from"),
			w = c.helpers.getSearchString("to");
		new l.
	default ({
			middle: {
				text: "方位更新",
				click: function() {}
			},
			right: {
				text: "导航",
				click: function() {
					window.location.href = window.location.href.split("views")[0] + "views/xzmapdh.html"
				}
			}
		});
		var d = new BMap.Map("allmap");
		d.centerAndZoom(new BMap.Point(p, w), 11), d.enableScrollWheelZoom(!0), d.disableDragging();
		new BMap.Icon("http://img1.imgtn.bdimg.com/it/u=146880881,2198480906&fm=21&gp=0.jpg", new BMap.Size(15, 15), {
			anchor: new BMap.Size(7.5, 7.5),
			imageOffset: new BMap.Size(0, 0)
		});
		setTimeout(function() {
			o()
		}, 1e3)
	}
});