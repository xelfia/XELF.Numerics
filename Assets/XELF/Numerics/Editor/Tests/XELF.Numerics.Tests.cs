// © 2018 XELF
// XELF.Numerics
// https://github.com/xelfia/XELF.Numerics

using NUnit.Framework;
using UnityEngine;
using XELF.Experimental.Numerics;

namespace Tests {
	public class NumericsTest {
		[Test]
		public void TestComplexNumber() {
			var r1 = new R(5);
			var r2 = new R(2);
			var r3 = new R(3);
			var j1 = new I(8);
			var d1 = new D(7);
			var d2 = new D(9);
			var w = r1 * (r2, r3);
			Debug.Log(w);
			Debug.Log(r1 + j1);
			Debug.Log(r1 - j1);
			Debug.Log(j1 * j1);
			Debug.Log(new RIJK((R)1, (I)2, (J)3, (K)4).Normalized);
			var a = r1 * (r1, j1);

			Debug.Log((RD)(r1, d1) * (r2, d2));
			Assert.AreEqual(-(R)6, (K)2 * (K)3);
			Assert.AreEqual(new RD(10, 45 + 14), (RD)(r1, d1) * (r2, d2));

			Assert.AreEqual(new R(25), r1 * r1);
			Assert.AreEqual(new R(5f / 2), r1 / r2);
			Assert.AreEqual(new R(0), d1 * d1);
			Assert.AreEqual((new R(25), new I(40)), a);
		}
		[Test]
		public void TestQuaternionUnit() {
			Assert.AreEqual(-(R)1, (I)1 * (J)1 * (K)1);
		}
		[Test]
		public void TestQuaternionMultiplication() {
			var f1 = 1f;
			var f2 = 2f;
			var f3 = 3f;
			var f4 = 4f;
			var f5 = -5f;
			var f6 = -6f;
			var f7 = -7f;
			var f8 = -8f;

			var q1 = new RIJK((I)f2, (J)f3, (K)f4, (R)f1).Normalized;
			var q2 = new RIJK((I)f6, (J)f7, (K)f8, (R)f5).Normalized;
			var q = q1 * q2;

			Debug.Log(q1.r * q2.ijk);
			Debug.Log(q2.r * q1.ijk);
			Debug.Log(IJK.Cross(q1.ijk, q2.ijk));
			var Q1 = new Quaternion(f2, f3, f4, f1).normalized;
			var Q2 = new Quaternion(f6, f7, f8, f5).normalized;
			var Q = Q1 * Q2;

			Debug.Log($"Mine: {q}");
			Debug.Log($"Unity: {Q}");
			var delta = 1e-4f;

			Assert.AreEqual(Q1.w, q1.r.w, delta);
			Assert.AreEqual(Q1.x, q1.i.x, delta);
			Assert.AreEqual(Q1.y, q1.j.y, delta);
			Assert.AreEqual(Q1.z, q1.k.z, delta);

			Assert.AreEqual(Q2.w, q2.r.w, delta);
			Assert.AreEqual(Q2.x, q2.i.x, delta);
			Assert.AreEqual(Q2.y, q2.j.y, delta);
			Assert.AreEqual(Q2.z, q2.k.z, delta);

			Assert.AreEqual(Q.w, q.r.w, delta);
			Assert.AreEqual(Q.x, q.i.x, delta);
			Assert.AreEqual(Q.y, q.j.y, delta);
			Assert.AreEqual(Q.z, q.k.z, delta);
		}
		[Test]
		public void TestDualQuaternionAddition() {
			var d1 = new RIJKD((R)1, (I)2, (J)3, (K)4, (D)5, (DI)6, (DJ)7, (DK)8);
			var d2 = new RIJKD((R)10, (I)11, (J)12, (K)13, (D)14, (DI)15, (DJ)16, (DK)17);
			var d = d1 + d2;

			Debug.Log(d);

			Assert.AreEqual((R)11, d.r);
			Assert.AreEqual((I)13, d.i);
			Assert.AreEqual((J)15, d.j);
			Assert.AreEqual((K)17, d.k);
			Assert.AreEqual((D)19, d.d);
			Assert.AreEqual((DI)21, d.di);
			Assert.AreEqual((DJ)23, d.dj);
			Assert.AreEqual((DK)25, d.dk);
		}
		[Test]
		public void TestDualQuaternionRotationPosition() {
			var r = new RIJK((I)1, (J)2, (K)3, (R)4).Normalized;
			var p = new Vector3(10, 20, 30);
			var dq = RIJKD.From(r, p);
			var p2 = dq.Position;
			var delta = 1e-4f;
			Assert.AreEqual(p.x, p2.x, delta);
			Assert.AreEqual(p.y, p2.y, delta);
			Assert.AreEqual(p.z, p2.z, delta);
		}
	}
}
