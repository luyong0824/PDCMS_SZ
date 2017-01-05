webpackJsonp([7], {
	0: function(e, t, i) {
		i(1), e.exports = i(332)
	},
	332: function(e, t, i) {
		"use strict";

		function a(e) {
			return e && e.__esModule ? e : {
			default:
				e
			}
		}
		function l() {
			(0, c.
		default)("#name").val(m)
		}
		function n() {
			var e = (0, c.
		default)(".img-list").find("ul"),
				t = e.eq(0),
				i = e.eq(1),
				a = e.eq(2),
				l = t.height(),
				n = i.height(),
				d = a.height();
			return l <= n ? l <= d ? t : a : n <= d ? i : a
		}
		function d() {
			c.
		default.ajax({
				url: h.config.url + "BaseStationBM/GetEngineeringProgressByIdMobile",
				type: "post",
				dataType: "json",
				data: {
					id: j,
					TypeId: 2,
					pageSize: 50
				},
				beforeSend: function() {
					g.
				default.show()
				},
				success: function(e) {
					if (console.log(e), "-2" == e.Code) return h.helpers.jump2login("../views/xmjddj.html?id=" + j + "&pn=" + m), !1;
					w = e.EngineeringProgress;
					var t = e.ImageUrl,
						i = e.ProgressMemos || "";
					M[w || "0"].selected = 1, (0, c.
				default)("#jd").val(M[w || "0"].text), (0, c.
				default)("#jdjs").text(i), b = new r.
				default ({
						dataList: M,
						cb: function(e, t, i) {
							w = t, (0, c.
						default)("#jd").val(e)
						}
					});
					var a = t.split(",");
					a.length > 0 && a.forEach(function(e, t) {
						if (3 != (0, c.
					default)(".img-wrap").find("ul").length)(0, c.
					default)(".img-wrap").append("<ul><li><img src=" + e + ' width="100%;" /><!--<span></span>--></li></ul>');
						else {
							var i = n();
							i.append("<li><img src=" + e + ' width="100%;" /><!--<span></span>--></li>')
						}
					}), (0, c.
				default)("#jdjs").val(i)
				},
				fail: function(e) {},
				complete: function(e) {
					g.
				default.hide()
				}
			})
		}
		i(299), i(301), i(333);
		var u = i(312),
			o = a(u),
			s = i(330),
			r = a(s),
			f = i(309),
			c = a(f),
			p = i(306),
			g = a(p),
			h = i(310),
			v = i(334);
		a(v);
		i(335), new o.
	default ({
			middle: {
				text: "工程进度登记"
			}
		});
		var m = h.helpers.getSearchString("pn"),
			j = h.helpers.getSearchString("id"),
			w = "",
			x = "",
			S = void 0,
			y = "",
			M = [{
				value: "",
				text: "请选择"
			}, {
				value: "1",
				text: "未开工"
			}, {
				value: "2",
				text: "进行中"
			}, {
				value: "3",
				text: "已完工"
			}, {
				value: "4",
				text: "暂缓"
			}, {
				value: "5",
				text: "取消"
			}],
			b = void 0;
		l(), (0, c.
	default)("#save").on("click", function() {
			return j ? w ? (x = c.
		default.trim((0, c.
		default)("#jdjs").val()), void c.
		default.ajax({
				url: h.config.url + "BaseStationBM/SaveProgressMobile",
				type: "post",
				dataType: "json",
				data: {
					data: JSON.stringify({
						Id: j,
						Base64String: y ? JSON.stringify(y) : "",
						EngineeringProgress: w,
						ProgressMemos: x,
						pageSize: 50
					}),
					TypeId: 2
				},
				beforeSend: function() {
					g.
				default.show()
				},
				success: function(e) {
					return "-2" == e.Code ? (h.helpers.jump2login("../views/xmjddj.html?id=" + j + "&pn=" + m), !1) : void("0" === e.Code ? (alert(e.Message), window.location.href = "./jddjlist.html?type=1") : alert("保存失败，请重试"))
				},
				fail: function(e) {},
				complete: function(e) {
					g.
				default.hide()
				}
			})) : (alert("请选择项目进度"), !1) : (alert("ID不能为空！"), !1)
		}), (0, c.
	default)(".select").on("click", function() {
			b.show()
		}), (0, c.
	default)("#imguploader").on("click", function() {
			(0, c.
		default)("#uploadphoto").click()
		}), (0, c.
	default)("#uploadphoto").localResizeIMG({
			width: 400,
			quality: .1,
			success: function(e) {
				S = e.base64;
				n();
				if (y || (y = []), y.push(S), 3 != (0, c.
			default)(".img-wrap").find("ul").length)(0, c.
			default)(".img-wrap").append("<ul><li><img src=" + S + ' width="100%;" /><!--<span></span>--></li></ul>');
				else {
					var t = n();
					t.append("<li><img src=" + S + ' width="100%;" /></li>')
				}
			}
		}), d()
	}
});