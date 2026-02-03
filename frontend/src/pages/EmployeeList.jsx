import { useState, useEffect } from 'react';

const API_BASE_URL = 'http://localhost:5192/api';

function EmployeeList() {
  const [employees, setEmployees] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    loadEmployees();
  }, []);

  const loadEmployees = async () => {
    try {
      setLoading(true);
      setError(null);

      const response = await fetch(`${API_BASE_URL}/employee`);
      if (!response.ok) throw new Error('Failed to fetch employees');

      const data = await response.json();
      
      // Get total expenses for each employee
      const employeesWithExpenses = await Promise.all(
        data.map(async (emp) => {
          try {
            const expResponse = await fetch(`${API_BASE_URL}/expense/total/employee/${emp.id}`);
            const total = expResponse.ok ? await expResponse.json() : 0;
            return { ...emp, totalExpenses: total };
          } catch {
            return { ...emp, totalExpenses: 0 };
          }
        })
      );

      setEmployees(employeesWithExpenses);
      setLoading(false);
    } catch (err) {
      console.error('Error loading employees:', err);
      setError(err.message);
      setLoading(false);
    }
  };

  const filteredEmployees = employees.filter(emp => {
    const search = searchTerm.toLowerCase();
    return (
      emp.firstName.toLowerCase().includes(search) ||
      emp.lastName.toLowerCase().includes(search) ||
      emp.email.toLowerCase().includes(search) ||
      emp.department.toLowerCase().includes(search)
    );
  });

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
        <h1 className="page-title">Employees</h1>
        <p className="page-subtitle">Manage employee information and expense totals</p>
      </div>

      {error && (
        <div className="alert alert-error">
          <svg width="20" height="20" viewBox="0 0 20 20" fill="none">
            <circle cx="10" cy="10" r="8" stroke="currentColor" strokeWidth="1.5"/>
            <path d="M10 6v4M10 13h.01" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
          </svg>
          <div>
            <strong>Error loading employees</strong>
            <p>{error}</p>
          </div>
        </div>
      )}

      <div className="card" style={{marginBottom: 'var(--space-lg)'}}>
        <div className="form-group" style={{marginBottom: 0}}>
          <label className="form-label">Search Employees</label>
          <input
            type="text"
            className="form-input"
            placeholder="Search by name, email, or department..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>
      </div>

      <div className="card">
        <div className="card-header">
          <h2 className="card-title">
            {filteredEmployees.length} Employee{filteredEmployees.length !== 1 ? 's' : ''}
          </h2>
        </div>

        {filteredEmployees.length === 0 ? (
          <div className="empty-state">
            <svg className="empty-state-icon" viewBox="0 0 64 64" fill="none">
              <circle cx="32" cy="24" r="10" stroke="currentColor" strokeWidth="2"/>
              <path d="M16 52c0-8.8 7.2-16 16-16s16 7.2 16 16" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/>
            </svg>
            <p>No employees found</p>
          </div>
        ) : (
          <div className="table-container">
            <table className="table">
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Email</th>
                  <th>Department</th>
                  <th>Hire Date</th>
                  <th>Total Expenses</th>
                </tr>
              </thead>
              <tbody>
                {filteredEmployees.map(employee => (
                  <tr key={employee.id}>
                    <td>
                      <div style={{display: 'flex', alignItems: 'center', gap: 'var(--space-md)'}}>
                        <div className="avatar" style={{width: '36px', height: '36px', fontSize: '0.75rem'}}>
                          {employee.firstName[0]}{employee.lastName[0]}
                        </div>
                        <div>
                          <div style={{fontWeight: 600}}>
                            {employee.firstName} {employee.lastName}
                          </div>
                          <div style={{fontSize: '0.75rem', color: 'var(--color-text-tertiary)'}}>
                            ID: {employee.id}
                          </div>
                        </div>
                      </div>
                    </td>
                    <td>
                      <a href={`mailto:${employee.email}`} style={{color: 'var(--color-accent)', textDecoration: 'none'}}>
                        {employee.email}
                      </a>
                    </td>
                    <td>
                      <span style={{
                        padding: '0.25rem 0.75rem',
                        background: 'var(--color-bg)',
                        borderRadius: '6px',
                        fontSize: '0.875rem',
                        fontWeight: 500
                      }}>
                        {employee.department}
                      </span>
                    </td>
                    <td>{new Date(employee.hireDate).toLocaleDateString()}</td>
                    <td>
                      <span style={{
                        fontFamily: 'var(--font-mono)',
                        fontWeight: 600,
                        fontSize: '0.9375rem'
                      }}>
                        ${employee.totalExpenses.toLocaleString('en-US', { minimumFractionDigits: 2 })}
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

export default EmployeeList;