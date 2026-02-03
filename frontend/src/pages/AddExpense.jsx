import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const API_BASE_URL = 'http://localhost:5192/api';

function AddExpense() {
  const navigate = useNavigate();
  const [employees, setEmployees] = useState([]);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(false);

  const [formData, setFormData] = useState({
    employeeId: '',
    categoryId: '',
    amount: '',
    expenseDate: new Date().toISOString().split('T')[0],
    description: ''
  });

  const [formErrors, setFormErrors] = useState({});

  useEffect(() => {
    loadFormData();
  }, []);

  const loadFormData = async () => {
    try {
      setLoading(true);
      const [empResponse, catResponse] = await Promise.all([
        fetch(`${API_BASE_URL}/employee`),
        fetch(`${API_BASE_URL}/employee/categories`)
      ]);

      if (!empResponse.ok || !catResponse.ok) {
        throw new Error('Failed to load form data');
      }

      const empData = await empResponse.json();
      const catData = await catResponse.json();

      setEmployees(empData);
      setCategories(catData);
      setLoading(false);
    } catch (err) {
      console.error('Error loading form data:', err);
      setError(err.message);
      setLoading(false);
    }
  };

  const validateForm = () => {
    const errors = {};

    if (!formData.employeeId) {
      errors.employeeId = 'Please select an employee';
    }

    if (!formData.categoryId) {
      errors.categoryId = 'Please select a category';
    }

    if (!formData.amount || parseFloat(formData.amount) <= 0) {
      errors.amount = 'Amount must be greater than 0';
    }

    if (!formData.expenseDate) {
      errors.expenseDate = 'Please select a date';
    }

    if (!formData.description || formData.description.trim().length === 0) {
      errors.description = 'Description is required';
    } else if (formData.description.length > 500) {
      errors.description = 'Description must be 500 characters or less';
    }

    setFormErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
    
    // Clear error for this field
    if (formErrors[name]) {
      setFormErrors(prev => ({ ...prev, [name]: undefined }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateForm()) {
      return;
    }

    try {
      setSubmitting(true);
      setError(null);

      const response = await fetch(`${API_BASE_URL}/expense`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          employeeId: parseInt(formData.employeeId),
          categoryId: parseInt(formData.categoryId),
          amount: parseFloat(formData.amount),
          expenseDate: formData.expenseDate,
          description: formData.description
        })
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Failed to create expense');
      }

      setSuccess(true);
      
      // Reset form
      setFormData({
        employeeId: '',
        categoryId: '',
        amount: '',
        expenseDate: new Date().toISOString().split('T')[0],
        description: ''
      });

      // Redirect after 2 seconds
      setTimeout(() => {
        navigate('/expenses');
      }, 2000);

    } catch (err) {
      console.error('Error submitting expense:', err);
      setError(err.message);
    } finally {
      setSubmitting(false);
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
        <h1 className="page-title">Add New Expense</h1>
        <p className="page-subtitle">Submit a new expense for approval</p>
      </div>

      {success && (
        <div className="alert alert-success">
          <svg width="20" height="20" viewBox="0 0 20 20" fill="none">
            <circle cx="10" cy="10" r="8" stroke="currentColor" strokeWidth="1.5"/>
            <path d="M7 10l2 2 4-4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/>
          </svg>
          <div>
            <strong>Success!</strong>
            <p>Expense created successfully. Redirecting to expenses list...</p>
          </div>
        </div>
      )}

      {error && (
        <div className="alert alert-error">
          <svg width="20" height="20" viewBox="0 0 20 20" fill="none">
            <circle cx="10" cy="10" r="8" stroke="currentColor" strokeWidth="1.5"/>
            <path d="M10 6v4M10 13h.01" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
          </svg>
          <div>
            <strong>Error creating expense</strong>
            <p>{error}</p>
          </div>
        </div>
      )}

      <div className="card">
        <form onSubmit={handleSubmit}>
          <div style={{display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(300px, 1fr))', gap: 'var(--space-lg)'}}>
            <div className="form-group">
              <label className="form-label required">Employee</label>
              <select
                name="employeeId"
                className="form-select"
                value={formData.employeeId}
                onChange={handleInputChange}
                disabled={submitting}
              >
                <option value="">Select an employee</option>
                {employees.map(emp => (
                  <option key={emp.id} value={emp.id}>
                    {emp.firstName} {emp.lastName} - {emp.department}
                  </option>
                ))}
              </select>
              {formErrors.employeeId && (
                <div className="form-error">{formErrors.employeeId}</div>
              )}
            </div>

            <div className="form-group">
              <label className="form-label required">Category</label>
              <select
                name="categoryId"
                className="form-select"
                value={formData.categoryId}
                onChange={handleInputChange}
                disabled={submitting}
              >
                <option value="">Select a category</option>
                {categories.map(cat => (
                  <option key={cat.id} value={cat.id}>
                    {cat.name}
                  </option>
                ))}
              </select>
              {formErrors.categoryId && (
                <div className="form-error">{formErrors.categoryId}</div>
              )}
            </div>

            <div className="form-group">
              <label className="form-label required">Amount</label>
              <input
                type="number"
                name="amount"
                className="form-input"
                placeholder="0.00"
                step="0.01"
                min="0"
                value={formData.amount}
                onChange={handleInputChange}
                disabled={submitting}
              />
              {formErrors.amount && (
                <div className="form-error">{formErrors.amount}</div>
              )}
            </div>

            <div className="form-group">
              <label className="form-label required">Expense Date</label>
              <input
                type="date"
                name="expenseDate"
                className="form-input"
                value={formData.expenseDate}
                onChange={handleInputChange}
                disabled={submitting}
              />
              {formErrors.expenseDate && (
                <div className="form-error">{formErrors.expenseDate}</div>
              )}
            </div>
          </div>

          <div className="form-group">
            <label className="form-label required">Description</label>
            <textarea
              name="description"
              className="form-textarea"
              placeholder="Enter a detailed description of the expense..."
              value={formData.description}
              onChange={handleInputChange}
              disabled={submitting}
            />
            <div style={{display: 'flex', justifyContent: 'space-between', marginTop: 'var(--space-xs)'}}>
              {formErrors.description && (
                <div className="form-error">{formErrors.description}</div>
              )}
              <div style={{fontSize: '0.8125rem', color: 'var(--color-text-tertiary)', marginLeft: 'auto'}}>
                {formData.description.length}/500
              </div>
            </div>
          </div>

          <div style={{display: 'flex', gap: 'var(--space-md)', marginTop: 'var(--space-xl)'}}>
            <button
              type="submit"
              className="btn btn-primary"
              disabled={submitting}
            >
              {submitting ? (
                <>
                  <div className="spinner" style={{width: '16px', height: '16px', borderWidth: '2px'}}></div>
                  Submitting...
                </>
              ) : (
                <>
                  <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
                    <path d="M8 3v10M3 8h10" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"/>
                  </svg>
                  Create Expense
                </>
              )}
            </button>
            <button
              type="button"
              className="btn btn-secondary"
              onClick={() => navigate('/expenses')}
              disabled={submitting}
            >
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default AddExpense;