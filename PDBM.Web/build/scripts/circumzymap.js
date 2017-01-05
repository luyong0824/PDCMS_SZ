webpackJsonp([3], {
	0: function(e, n, i) {
		i(1), e.exports = i(319)
	},
	319: function(e, n, i) {
		"use strict";

		function t(e) {
			return e && e.__esModule ? e : {
			default:
				e
			}
		}
		function o() {
			w.clearOverlays();
			var e = new BMap.Point(r, p),
				n = new BMap.Marker(e),
				i = null;
			n.addEventListener("click", function() {
				window.location.href = window.location.href.split("views")[0] + "views/circumdetail.html"
			}), i = new BMap.Label("测试测试", {
				offset: new BMap.Size(-10, -32)
			}), i.setStyle({
				borderColor: "#ccc",
				fontSize: "14px",
				color: "#333",
				borderRadius: "3px",
				padding: "6px 4px"
			}), n.setLabel(i), w.addOverlay(n), w.panTo(e)
		}
		i(299), i(301), i(302);
		var a = i(305),
			l = (t(a), i(312)),
			c = t(l);
		new c.
	default ({
			middle: {
				text: "周边资源"
			},
			right: {
				text: "盲点反馈",
				click: function() {
					window.location.href = window.location.href.split("views")[0] + "views/md.html"
				}
			}
		});
		var r = "120.578085",
			p = "31.263001",
			w = new BMap.Map("allmap");
		w.centerAndZoom(new BMap.Point(116.331398, 39.897445), 11), w.enableScrollWheelZoom(!0), w.disableDragging();
		new BMap.Icon("http://img1.imgtn.bdimg.com/it/u=146880881,2198480906&fm=21&gp=0.jpg", new BMap.Size(15, 15), {
			anchor: new BMap.Size(7.5, 7.5),
			imageOffset: new BMap.Size(0, 0)
		});
		setTimeout(function() {
			o()
		}, 1e3)
	}
});