import { useState, useEffect } from 'react';

const API_BASE_URL = 'http://localhost:5192/api';

function ExpenseList() {
  const [expenses, setExpenses] = useState([]);
  const [filteredExpenses, setFilteredExpenses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  const [filters, setFilters] = useState({
    status: 'all',
    search: '',
    dateFrom: '',
    dateTo: ''
  });

  useEffect(() => {
    loadExpenses();
  }, []);

  useEffect(() => {
    applyFilters();
  }, [filters, expenses]);

  const loadExpenses = async () => {
    try {
      setLoading(true);
      setError(null);

      const response = await fetch(`${API_BASE_URL}/expense`);
      if (!response.ok) throw new Error('Failed to fetch expenses');

      const data = await response.json();
      setExpenses(data);
      setLoading(false);
    } catch (err) {
      console.error('Error loading expenses:', err);
      setError(err.message);
      setLoading(false);
    }
  };

  const applyFilters = () => {
    let filtered = [...expenses];

    // Status filter
    if (filters.status !== 'all') {
      filtered = filtered.filter(e => e.status.toLowerCase() === filters.status);
    }

    // Search filter
    if (filters.search) {
      const search = filters.search.toLowerCase();
      filtered = filtered.filter(e =>
        e.description.toLowerCase().includes(search) ||
        e.employeeName.toLowerCase().includes(search) ||
        e.categoryName.toLowerCase().includes(search)
      );
    }

    // Date range filter
    if (filters.dateFrom) {
      filtered = filtered.filter(e => new Date(e.expenseDate) >= new Date(filters.dateFrom));
    }
    if (filters.dateTo) {
      filtered = filtered.filter(e => new Date(e.expenseDate) <= new Date(filters.dateTo));
    }

    setFilteredExpenses(filtered);
  };

  const handleFilterChange = (key, value) => {
    setFilters(prev => ({ ...prev, [key]: value }));
  };

  const handleDelete = async (id) => {
    if (!confirm('Are you sure you want to delete this expense?')) return;

    try {
      const response = await fetch(`${API_BASE_URL}/expense/${id}`, {
        method: 'DELETE'
      });

      if (!response.ok) throw new Error('Failed to delete expense');

      setExpenses(prev => prev.filter(e => e.id !== id));
    } catch (err) {
      alert('Error deleting expense: ' + err.message);
    }
  };

  const handleStatusUpdate = async (id, newStatus) => {
    try {
      const response = await fetch(`${API_BASE_URL}/expense/${id}/status`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ status: newStatus })
      });

      if (!response.ok) throw new Error('Failed to update status');

      setExpenses(prev => prev.map(e => 
        e.id === id ? { ...e, status: newStatus } : e
      ));
    } catch (err) {
      alert('Error updating status: ' + err.message);
    }
  };

  const exportToExcel = async () => {
    try {
      const params = new URLSearchParams();
      if (filters.dateFrom) params.append('startDate', filters.dateFrom);
      if (filters.dateTo) params.append('endDate', filters.dateTo);

      const response = await fetch(`${API_BASE_URL}/expense/export/excel?${params}`);
      if (!response.ok) throw new Error('Failed to export');

      const blob = await response.blob();
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `expenses-${new Date().toISOString().split('T')[0]}.xlsx`;
      a.click();
      window.URL.revokeObjectURL(url);
    } catch (err) {
      alert('Error exporting: ' + err.message);
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

  return (
    <div className="page">
      <div className="page-header">
        <h1 className="page-title">Expenses</h1>
        <p className="page-subtitle">Manage and review all expense submissions</p>
      </div>

      {error && (
        <div className="alert alert-error">
          <svg width="20" height="20" viewBox="0 0 20 20" fill="none">
            <circle cx="10" cy="10" r="8" stroke="currentColor" strokeWidth="1.5"/>
            <path d="M10 6v4M10 13h.01" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
          </svg>
          <div>
            <strong>Error loading expenses</strong>
            <p>{error}</p>
          </div>
        </div>
      )}

      <div className="card" style={{marginBottom: 'var(--space-lg)'}}>
        <div style={{display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: 'var(--space-lg)'}}>
          <div className="form-group" style={{marginBottom: 0}}>
            <label className="form-label">Search</label>
            <input
              type="text"
              className="form-input"
              placeholder="Description, employee, category..."
              value={filters.search}
              onChange={(e) => handleFilterChange('search', e.target.value)}
            />
          </div>

          <div className="form-group" style={{marginBottom: 0}}>
            <label className="form-label">Status</label>
            <select
              className="form-select"
              value={filters.status}
              onChange={(e) => handleFilterChange('status', e.target.value)}
            >
              <option value="all">All Statuses</option>
              <option value="pending">Pending</option>
              <option value="approved">Approved</option>
              <option value="rejected">Rejected</option>
            </select>
          </div>

          <div className="form-group" style={{marginBottom: 0}}>
            <label className="form-label">From Date</label>
            <input
              type="date"
              className="form-input"
              value={filters.dateFrom}
              onChange={(e) => handleFilterChange('dateFrom', e.target.value)}
            />
          </div>

          <div className="form-group" style={{marginBottom: 0}}>
            <label className="form-label">To Date</label>
            <input
              type="date"
              className="form-input"
              value={filters.dateTo}
              onChange={(e) => handleFilterChange('dateTo', e.target.value)}
            />
          </div>
        </div>

        <div style={{marginTop: 'var(--space-lg)', display: 'flex', gap: 'var(--space-sm)'}}>
          <button className="btn btn-secondary" onClick={() => setFilters({ status: 'all', search: '', dateFrom: '', dateTo: '' })}>
            Clear Filters
          </button>
          <button className="btn btn-primary" onClick={exportToExcel}>
            <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
              <path d="M8 2v8M5 7l3 3 3-3" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
              <path d="M3 14h10" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
            </svg>
            Export to Excel
          </button>
        </div>
      </div>

      <div className="card">
        <div className="card-header">
          <h2 className="card-title">
            {filteredExpenses.length} Expense{filteredExpenses.length !== 1 ? 's' : ''}
          </h2>
        </div>

        {filteredExpenses.length === 0 ? (
          <div className="empty-state">
            <svg className="empty-state-icon" viewBox="0 0 64 64" fill="none">
              <circle cx="32" cy="32" r="24" stroke="currentColor" strokeWidth="2"/>
              <path d="M22 32h20M32 22v20" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/>
            </svg>
            <p>No expenses found matching your filters</p>
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
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {filteredExpenses.map(expense => (
                  <tr key={expense.id}>
                    <td>
                      <div style={{fontWeight: 600, marginBottom: '0.25rem'}}>{expense.description}</div>
                      <div style={{fontSize: '0.75rem', color: 'var(--color-text-tertiary)'}}>
                        ID: {expense.id}
                      </div>
                    </td>
                    <td>{expense.employeeName}</td>
                    <td>{expense.categoryName}</td>
                    <td>
                      <span style={{fontFamily: 'var(--font-mono)', fontWeight: 600, fontSize: '0.9375rem'}}>
                        ${expense.amount.toLocaleString('en-US', { minimumFractionDigits: 2 })}
                      </span>
                    </td>
                    <td>{new Date(expense.expenseDate).toLocaleDateString()}</td>
                    <td>
                      <select
                        className="badge"
                        style={{
                          border: 'none',
                          cursor: 'pointer',
                          fontWeight: 600
                        }}
                        value={expense.status}
                        onChange={(e) => handleStatusUpdate(expense.id, e.target.value)}
                      >
                        <option value="Pending">PENDING</option>
                        <option value="Approved">APPROVED</option>
                        <option value="Rejected">REJECTED</option>
                      </select>
                    </td>
                    <td>
                      <button
                        className="btn btn-ghost"
                        onClick={() => handleDelete(expense.id)}
                        style={{padding: '0.5rem'}}
                      >
                        <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
                          <path d="M3 4h10M5 4V3a1 1 0 011-1h4a1 1 0 011 1v1M6 7v4M10 7v4M4 4h8l-1 9H5L4 4z" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
                        </svg>
                      </button>
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

export default ExpenseList;