import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

const API_BASE_URL = 'http://localhost:5192/api';

function Dashboard() {
  const [stats, setStats] = useState(null);
  const [recentExpenses, setRecentExpenses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      setError(null);

      // Fetch summary data
      const [categoryResponse, statusResponse, expensesResponse] = await Promise.all([
        fetch(`${API_BASE_URL}/expense/summary/category`),
        fetch(`${API_BASE_URL}/expense/summary/status`),
        fetch(`${API_BASE_URL}/expense`)
      ]);

      if (!categoryResponse.ok || !statusResponse.ok || !expensesResponse.ok) {
        throw new Error('Failed to fetch dashboard data');
      }

      const categoryData = await categoryResponse.json();
      const statusData = await statusResponse.json();
      const expensesData = await expensesResponse.json();

      // Calculate total expenses
      const totalAmount = expensesData.reduce((sum, exp) => sum + exp.amount, 0);
      
      // Get status counts
      const pendingCount = statusData['Pending'] || 0;
      const approvedCount = statusData['Approved'] || 0;

      setStats({
        totalExpenses: totalAmount,
        pendingCount,
        approvedCount,
        totalCount: expensesData.length
      });

      // Get most recent 5 expenses
      setRecentExpenses(
        expensesData
          .sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))
          .slice(0, 5)
      );

      setLoading(false);
    } catch (err) {
      console.error('Dashboard error:', err);
      setError(err.message);
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="page">
        <div className="loading">
          <div className="spinner"></div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="page">
        <div className="alert alert-error">
          <svg width="20" height="20" viewBox="0 0 20 20" fill="none">
            <circle cx="10" cy="10" r="8" stroke="currentColor" strokeWidth="1.5"/>
            <path d="M10 6v4M10 13h.01" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
          </svg>
          <div>
            <strong>Error loading dashboard</strong>
            <p>{error}</p>
            <button className="btn btn-ghost" onClick={loadDashboardData} style={{marginTop: '0.5rem'}}>
              Try Again
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="page">
      <div className="page-header">
        <h1 className="page-title">Dashboard</h1>
        <p className="page-subtitle">Overview of your expense reporting activity</p>
      </div>

      <div className="stats-grid">
        <div className="stat-card" style={{'--stat-color': '#E63946'}}>
          <div className="stat-label">Total Expenses</div>
          <div className="stat-value">${stats.totalExpenses.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</div>
          <div className="stat-change">
            {stats.totalCount} total submissions
          </div>
        </div>

        <div className="stat-card" style={{'--stat-color': '#F59E0B'}}>
          <div className="stat-label">Pending Review</div>
          <div className="stat-value">{stats.pendingCount}</div>
          <div className="stat-change">
            Awaiting approval
          </div>
        </div>

        <div className="stat-card" style={{'--stat-color': '#06B6D4'}}>
          <div className="stat-label">Approved</div>
          <div className="stat-value">{stats.approvedCount}</div>
          <div className="stat-change positive">
            ✓ Processed
          </div>
        </div>

        <div className="stat-card" style={{'--stat-color': '#8B5CF6'}}>
          <div className="stat-label">This Month</div>
          <div className="stat-value">
            {recentExpenses.filter(e => {
              const expDate = new Date(e.expenseDate);
              const now = new Date();
              return expDate.getMonth() === now.getMonth() && 
                     expDate.getFullYear() === now.getFullYear();
            }).length}
          </div>
          <div className="stat-change">
            Expenses submitted
          </div>
        </div>
      </div>

      <div className="card">
        <div className="card-header">
          <h2 className="card-title">Recent Expenses</h2>
          <Link to="/expenses" className="btn btn-ghost">
            View All →
          </Link>
        </div>

        {recentExpenses.length === 0 ? (
          <div className="empty-state">
            <svg className="empty-state-icon" viewBox="0 0 64 64" fill="none">
              <rect x="12" y="16" width="40" height="32" rx="2" stroke="currentColor" strokeWidth="2"/>
              <path d="M20 24h24M20 32h24M20 40h16" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/>
            </svg>
            <p>No expenses found</p>
            <Link to="/add-expense" className="btn btn-primary" style={{marginTop: '1rem'}}>
              Add Your First Expense
            </Link>
          </div>
        ) : (
          <div className="table-container">
            <table className="table">
              <thead>
                <tr>
                  <th>Description</th>
                  <th>Employee</th>
                  <th>Category</th>
                  <th>Amount</th>
                  <th>Date</th>
                  <th>Status</th>
                </tr>
              </thead>
              <tbody>
                {recentExpenses.map(expense => (
                  <tr key={expense.id}>
                    <td>
                      <div style={{fontWeight: 600}}>{expense.description}</div>
                    </td>
                    <td>{expense.employeeName}</td>
                    <td>{expense.categoryName}</td>
                    <td>
                      <span style={{fontFamily: 'var(--font-mono)', fontWeight: 600}}>
                        ${expense.amount.toLocaleString('en-US', { minimumFractionDigits: 2 })}
                      </span>
                    </td>
                    <td>{new Date(expense.expenseDate).toLocaleDateString()}</td>
                    <td>
                      <span className={`badge badge-${expense.status.toLowerCase()}`}>
                        {expense.status}
                      </span>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}

export default Dashboard;