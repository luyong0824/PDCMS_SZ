webpackJsonp([2], {
	0: function(e, n, a) {
		a(1), e.exports = a(318)
	},
	318: function(e, n, a) {
		"use strict";

		function i(e) {
			return e && e.__esModule ? e : {
			default:
				e
			}
		}
		function t() {
			d.clearOverlays();
			var e = new BMap.Point(p, c),
				n = new BMap.Marker(e),
				a = null;
			n.addEventListener("click", function() {
				window.location.href = window.location.href.split("views")[0] + "views/circumdetail.html"
			}), a = new BMap.Label("测试测试", {
				offset: new BMap.Size(-10, -32)
			}), a.setStyle({
				borderColor: "#ccc",
				fontSize: "14px",
				color: "#333",
				borderRadius: "3px",
				padding: "6px 4px"
			}), n.setLabel(a), d.addOverlay(n), d.panTo(e)
		}
		a(299), a(301), a(302);
		var o = a(305),
			l = (i(o), a(312)),
			r = i(l);
		new r.
	default ({
			middle: {
				text: "周边资源"
			}
		});
		var p = "120.578085",
			c = "31.263001",
			d = new BMap.Map("allmap");
		d.centerAndZoom(new BMap.Point(116.331398, 39.897445), 11), d.enableScrollWheelZoom(!0), d.disableDragging();
		new BMap.Icon("http://img1.imgtn.bdimg.com/it/u=146880881,2198480906&fm=21&gp=0.jpg", new BMap.Size(15, 15), {
			anchor: new BMap.Size(7.5, 7.5),
			imageOffset: new BMap.Size(0, 0)
		});
		setTimeout(function() {
			t()
		}, 1e3)
	}
});