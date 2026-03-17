// JJ
// SurfaceSpline.cs
// NASA Harder-Desmarais Surface Spline (1972) – fully compatible with your WPF project

using System;
using System.Collections.Generic;
using System.Windows;                    // ← This is the correct Point for OrthoProjectionDraw

namespace OrthoProjectionDraw
{
    public class SurfaceSpline
    {
        private readonly List<Point> _controlPoints = new();   // WPF Point
        private readonly List<double> _lambdas = new();
        private double _a0, _a1, _a2;

        public void Fit(IList<Point> points, IList<double> values)   // ← WPF Point
        {
            int n = points.Count;
            if (n < 3) throw new ArgumentException("Need at least 3 points for surface spline.");

            _controlPoints.Clear();
            _controlPoints.AddRange(points);

            double[,] A = new double[n + 3, n + 3];
            double[] b = new double[n + 3];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j) continue;
                    double dx = points[i].X - points[j].X;
                    double dy = points[i].Y - points[j].Y;
                    double r2 = dx * dx + dy * dy;
                    A[i, j] = (r2 < 1e-20) ? 0.0 : r2 * Math.Log(r2);
                }

                A[i, n] = 1.0;
                A[i, n + 1] = points[i].X;
                A[i, n + 2] = points[i].Y;

                b[i] = values[i];
            }

            for (int j = 0; j < n; j++)
            {
                A[n, j] = 1.0;
                A[n + 1, j] = points[j].X;
                A[n + 2, j] = points[j].Y;
            }

            double[] coeffs = SolveGaussian(A, b);

            _lambdas.Clear();
            for (int i = 0; i < n; i++) _lambdas.Add(coeffs[i]);

            _a0 = coeffs[n];
            _a1 = coeffs[n + 1];
            _a2 = coeffs[n + 2];
        }

        public double Evaluate(double x, double y)
        {
            double sum = _a0 + _a1 * x + _a2 * y;

            for (int i = 0; i < _controlPoints.Count; i++)
            {
                double dx = x - _controlPoints[i].X;
                double dy = y - _controlPoints[i].Y;
                double r2 = dx * dx + dy * dy;
                if (r2 < 1e-20) continue;
                sum += _lambdas[i] * r2 * Math.Log(r2);
            }
            return sum;
        }

        private static double[] SolveGaussian(double[,] A, double[] b)
        {
            int n = b.Length;
            double[,] M = new double[n, n + 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) M[i, j] = A[i, j];
                M[i, n] = b[i];
            }

            // Forward elimination with partial pivoting
            for (int i = 0; i < n; i++)
            {
                int max = i;
                for (int k = i + 1; k < n; k++)
                    if (Math.Abs(M[k, i]) > Math.Abs(M[max, i])) max = k;

                for (int j = 0; j <= n; j++)
                {
                    double tmp = M[max, j];
                    M[max, j] = M[i, j];
                    M[i, j] = tmp;
                }

                for (int k = i + 1; k < n; k++)
                {
                    double c = -M[k, i] / M[i, i];
                    for (int j = i; j <= n; j++)
                        M[k, j] += (j == i ? 0 : c * M[i, j]);
                }
            }

            // Back substitution
            double[] x = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                x[i] = M[i, n];
                for (int j = i + 1; j < n; j++)
                    x[i] -= M[i, j] * x[j];
                x[i] /= M[i, i];
            }
            return x;
        }
    }
} //namespace 